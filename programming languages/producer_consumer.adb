-- A skeleton of an ADA program for an assignment in programming languages
--Antoni Czuba 201096, Jan Moniewski 199074, Michal Kulesza 198346
with Ada.Text_IO; use Ada.Text_IO;
with Ada.Strings; use Ada.Strings;
with Ada.Strings.Unbounded; use Ada.Strings.Unbounded;
with Ada.Characters.Latin_1; use Ada.Characters.Latin_1;
with Ada.Integer_Text_IO;
with Ada.Numerics.Discrete_Random;


procedure main is

   ----GLOBAL VARIABLES---

   Number_Of_Producers: constant Integer := 5;
   Number_Of_Assemblies: constant Integer := 3;
   Number_Of_Consumers: constant Integer := 2;
   Delivery_size: constant Integer := 2;

   subtype Producer_Type is Integer range 1 .. Number_Of_Producers;
   subtype Assembly_Type is Integer range 1 .. Number_Of_Assemblies;
   subtype Consumer_Type is Integer range 1 .. Number_Of_Consumers;
   type Storage_type is array (Producer_Type) of Integer;


   --each Producer is assigned a Product that it produces
   Product_Name: constant array (Producer_Type) of Unbounded_String
     := (To_Unbounded_String("Pita"),  --1
         To_Unbounded_String("Lawasz"), --2
         To_Unbounded_String("Kurczak"), --3
         To_Unbounded_String("Wolowina"), --4
         To_Unbounded_String("Kapusta")); --5

   --Assembly is a collection of products
   Assembly_Name: constant array (Assembly_Type) of Unbounded_String
     := (To_Unbounded_String("Mieso mieszane P"), --1
         To_Unbounded_String("Kurczak L"), --2
         To_Unbounded_String("Wolowina L")); --3


   ----TASK DECLARATIONS----

   -- Producer produces determined product
   task type Producer is
      entry Start(Product: in Producer_Type; Production_Time: in Integer);

      entry ResumeProd;
   end Producer;

   -- Consumer gets an arbitrary assembly of several products from the buffer
   -- but he/she orders it randomly
   task type Consumer is
      entry Start(Consumer_Number: in Consumer_Type;
                  Consumption_Time: in Integer);
   end Consumer;

   task type Inspector is
      entry Start;
      end Inspector;

   -- Buffer receives products from Producers and delivers Assemblies to Consumers
   task type Buffer is
      -- Accept a product to the storage (provided there is a room for it)
      entry Take(Product: in Producer_Type; Number: in Integer; Is_storage_full: out Boolean);
      -- Deliver an assembly (provided there are enough products for it)
      entry Deliver(Assembly: in Assembly_Type; Number: out Integer; IsRepeated: in Boolean; Lacking_assembly:in out Storage_type);
      entry Inspection_In_Storage;
   end Buffer;

   P: array ( 1 .. Number_Of_Producers ) of Producer;
   K: array ( 1 .. Number_Of_Consumers ) of Consumer;
   B: Buffer;
   I: Inspector;

   ----TASK DEFINITIONS----

   --Producer--
   task body Producer is
      subtype Production_Time_Range is Integer range 3 .. 5;
      package Random_Production is new Ada.Numerics.Discrete_Random(Production_Time_Range);
      --  random number generator
      G: Random_Production.Generator;
      Producer_Type_Number: Integer;
      Product_Number: Integer;
      Production: Integer;
      Random_Time: Duration;
      Is_storage_full: Boolean;
   begin
      accept Start(Product: in Producer_Type; Production_Time: in Integer) do
         --  start random number generator
         Random_Production.Reset(G);
         Product_Number := 1;
         Producer_Type_Number := Product;
         Production := Production_Time;
         Is_storage_full:= False;
      end Start;
      Put_Line(ESC & "[93m" & "PRODUCENT: Rozpoczeto produkcje " & To_String(Product_Name(Producer_Type_Number)) & ESC & "[0m");
      loop
         Is_storage_full:=false;
         while Is_storage_full=false loop
            Random_Time := Duration(Random_Production.Random(G));
            delay Random_Time;
            Put_Line(ESC & "[93m" & "PRODUCENT: Wyprodukowano " & To_String(Product_Name(Producer_Type_Number))
                     & " numer "  & Integer'Image(Product_Number) & ESC & "[0m");
            -- Accept for storage
            B.Take(Producer_Type_Number, Product_Number, Is_storage_full);
            Product_Number := Product_Number + 1;
            if Is_storage_full then
               Put_Line(ESC & "[93m" & "PRODUCENT: Zatrzymano produkcje " & To_String(Product_Name(Producer_Type_Number)) & ESC & "[0m");
            end if;
         end loop;
         while Is_storage_full loop
            select
               accept ResumeProd  do
                  Put_Line(ESC & "[93m" & "PRODUCENT: Wznowiono produkcje " & To_String(Product_Name(Producer_Type_Number)) & ESC & "[0m");
                  Is_storage_full:=False;
               end ResumeProd;
            else
               delay 0.1;
            end select;
         end loop;
      end loop;
   end Producer;

   --Inspector--

   task body Inspector is
      subtype Waiting_Time_Range is Integer range 7 .. 10;
      package Random_Waiting is new Ada.Numerics.Discrete_Random(Waiting_Time_Range);

      Inspection_time: Integer;
      Pause: Random_Waiting.Generator;
   begin
      accept Start  do
         Inspection_time:= 3;
         Random_Waiting.Reset(Pause);
         Put_Line(ESC & "[96m" & "INSPEKTOR: Zaczyna prace " & ESC & "[0m");
      end Start;
      loop
         delay Duration(Random_Waiting.Random(Pause));
         Put_Line(ESC & "[96m" & "INSPEKTOR: Zaczynam inspekcje magazynu " & ESC & "[0m");
         delay Duration(Inspection_time);
         B.Inspection_In_Storage;
      end loop;
   end Inspector;


   --Consumer--

   task body Consumer is
      subtype Between_Time_Range is Integer range 4 .. 8;
      subtype Waiting_for_assembly_Time_Range is Integer range 2 .. 6;
      package Random_time_between is new
        Ada.Numerics.Discrete_Random(Between_Time_Range);
      package Waiting_for_assembly_between is new
        Ada.Numerics.Discrete_Random(Waiting_for_assembly_Time_Range);

      --each Consumer takes any (random) Assembly from the Buffer
      package Random_Assembly is new
        Ada.Numerics.Discrete_Random(Assembly_Type);

      G: Random_time_between.Generator;
      GW: Waiting_for_assembly_between.Generator;
      GA: Random_Assembly.Generator;
      Consumer_Nb: Consumer_Type;
      Assembly_Number: Integer;
      Assembly_Type: Integer;
      Lacking_assembly: Storage_type;
      Consumer_Name: constant array (1 .. Number_Of_Consumers)
        of Unbounded_String
        := (To_Unbounded_String("Amelia Radecka"),
            To_Unbounded_String("Lukasz Pietras"));
   begin
      accept Start(Consumer_Number: in Consumer_Type;
                   Consumption_Time: in Integer) do
         Random_time_between.Reset(G);
         Waiting_for_assembly_between.Reset(GW);
         Random_Assembly.Reset(GA);
         Consumer_Nb := Consumer_Number;

      end Start;
      Put_Line(ESC & "[96m" & "KLIENT: " & To_String(Consumer_Name(Consumer_Nb)) & " ma ochote cos przkasic" & ESC & "[0m");
      loop
         Lacking_assembly:= (0,0,0,0,0);
         delay Duration(Random_time_between.Random(G)); --  simulate consumption
         Assembly_Type := Random_Assembly.Random(GA);
         -- take an assembly for consumption
         Put_Line(ESC & "[96m" & "KLIENT: " & To_String(Consumer_Name(Consumer_Nb)) & " wchodzi do lokalu i zamawia " &
                    To_String(Assembly_Name(Assembly_Type)) &  ESC & "[0m");
         B.Deliver(Assembly_Type, Assembly_Number,False,Lacking_assembly);
         if Assembly_Number=0 then
            delay Duration(Waiting_for_assembly_between.Random(GW));
            B.Deliver(Assembly_Type, Assembly_Number,True,Lacking_assembly);
            if Assembly_Number=0 then
               Put_Line(ESC & "[91m" & "KLIENT: " & To_String(Consumer_Name(Consumer_Nb)) & " wychodzi z lokalu smutny..." & ESC & "[0m");
            end if;
         end if;
      end loop;
   end Consumer;


   --Buffer--

   task body Buffer is
      Storage_Capacity: constant Integer := 50;
      Storage_Capacity_for_single_item: constant Integer := 10;
      Storage: Storage_type
        := (0, 0, 0, 0, 0);
      Assembly_Content: array(Assembly_Type, Producer_Type) of Integer
        := ((1, 0, 1, 1, 0),
            (0, 1, 2, 0, 1),
            (0, 1, 0, 2, 1));
      Max_Assembly_Content: array(Producer_Type) of Integer;
      Assembly_Number: array(Assembly_Type) of Integer
        := (1, 1, 1);
      In_Storage: Integer := 0;

      procedure Setup_Variables is
      begin
         for W in Producer_Type loop
            Max_Assembly_Content(W) := 0;
            for Z in Assembly_Type loop
               if Assembly_Content(Z, W) > Max_Assembly_Content(W) then
                  Max_Assembly_Content(W) := Assembly_Content(Z, W);
               end if;
            end loop;
         end loop;
      end Setup_Variables;

      function IsLimit(Product: Producer_Type) return Boolean is
      begin
         if Storage(Product) >= Storage_Capacity_for_single_item then
            return True;
         else
            return False;
         end if;
      end IsLimit;


      function Can_Accept(Product: Producer_Type) return Boolean is
      begin
         if In_Storage >= Storage_Capacity then
            return False;
         else
            return True;
         end if;
      end Can_Accept;

      function Can_Deliver(Assembly: Assembly_Type; Lacking_assembly: in out Storage_type;  IsRepeated: in Boolean) return Boolean is
         returnVal: Boolean:= True;
      begin
         if IsRepeated then
            return False;
         end if;
         for W in Producer_Type loop
            if Storage(W) < Assembly_Content(Assembly, W) then
               Lacking_assembly(W):=Assembly_Content(Assembly,W)-Storage(W);
               returnVal:= False;
            end if;
         end loop;
         if returnVal=False then
            for W in Producer_Type loop
               --Put_Line(ESC & "[91m" & " odejmowanie ze storage " & Integer'Image(Assembly_Content(Assembly, W)) & " " & Integer'Image(Lacking_assembly(W)) & ESC & "[0m");
               Storage(W):=Storage(W) - Assembly_Content(Assembly, W) + Lacking_assembly(W);
               In_Storage:=In_Storage - Assembly_Content(Assembly, W) + Lacking_assembly(W);
            end loop;
         end if;
         return returnVal;
      end Can_Deliver;

      function Can_Deliver_Second_Time(Assembly: Assembly_Type; Lacking_assembly: in  Storage_type) return Boolean is
      begin
         for W in Producer_Type loop
            if Storage(W) < Lacking_assembly(W) then
               return False;
            end if;
         end loop;
         return True;
      end Can_Deliver_Second_Time;

      procedure Storage_Contents is
      begin
         Put_Line("|   Zawartosc magazynu " & Integer'Image(Storage(1)) & " " & To_String(Product_Name(1)) & Ada.Characters.Latin_1.LF &
                    "|   Zawartosc magazynu " & Integer'Image(Storage(2)) & " " & To_String(Product_Name(2)) & Ada.Characters.Latin_1.LF &
                    "|   Zawartosc magazynu " & Integer'Image(Storage(3)) & " " & To_String(Product_Name(3)) & Ada.Characters.Latin_1.LF &
                    "|   Zawartosc magazynu " & Integer'Image(Storage(4)) & " " & To_String(Product_Name(4)) & Ada.Characters.Latin_1.LF &
                    "|   Zawartosc magazynu " & Integer'Image(Storage(5)) & " " & To_String(Product_Name(5)) & Ada.Characters.Latin_1.LF &
                    "|   Ilosc produktow w magazyn " & Integer'Image(In_Storage));

      end Storage_Contents;

      procedure Taking_Ingredients(W: in Producer_Type;Subtrahend: in Integer; Assembly: in Assembly_Type) is
      begin
         if Storage(W) = Storage_Capacity_for_single_item and Assembly_Content(Assembly, W) > 0 then
            In_Storage := In_Storage - Subtrahend;
            Storage(W) := Storage(W) - Subtrahend;
            P(W).ResumeProd;
         elsif Storage(W) = Storage_Capacity_for_single_item + 1 and Assembly_Content(Assembly, W) > 1 then
            In_Storage := In_Storage - Subtrahend;
            Storage(W) := Storage(W) - Subtrahend;
            P(W).ResumeProd;
         else
            In_Storage := In_Storage - Subtrahend;
            Storage(W) := Storage(W) - Subtrahend;
         end if;
      end Taking_Ingredients;


   begin
      Put_Line(ESC & "[96m" & "MIM: Otwarto kebab MIM we wrzeszczu PKP" & ESC & "[0m");
      Setup_Variables;
      loop
         select
            accept Take(Product: in Producer_Type; Number: in Integer; Is_storage_full: out Boolean) do
               Is_storage_full:=False;
               if Can_Accept(Product) then
                  Put_Line(ESC & "[96m" & "MIM: Zaakceptowano produkt " & To_String(Product_Name(Product)) & " numer " &
                             Integer'Image(Number)& ESC & "[0m");
                  Storage(Product) := Storage(Product) + Delivery_size;
                  Is_storage_full:= IsLimit(Product);
                  In_Storage := In_Storage + Delivery_size;
               else
                  Put_Line(ESC & "[91m" & "MIM: Odrzucono produkt " & To_String(Product_Name(Product)) & " numer " &
                             Integer'Image(Number)& ESC & "[0m");
               end if;
            end Take;
         or
            accept Deliver(Assembly: in Assembly_Type; Number: out Integer; IsRepeated: in Boolean; Lacking_assembly:in out Storage_type) do
               if Can_Deliver(Assembly, Lacking_assembly, IsRepeated) = True and IsRepeated = False then
                  Put_Line(ESC & "[96m" & "MIM: Dostarczono " & To_String(Assembly_Name(Assembly)) & " numer " &
                             Integer'Image(Assembly_Number(Assembly))& ESC & "[0m");
                  for W in Producer_Type loop
                     Taking_Ingredients(W,Assembly_Content(Assembly, W),Assembly);
                  end loop;
                  Number := Assembly_Number(Assembly);
                  Assembly_Number(Assembly) := Assembly_Number(Assembly) + 1;
               elsif IsRepeated = True then
                  if Can_Deliver_Second_Time(Assembly, Lacking_assembly) then
                     Put_Line(ESC & "[96m" & "MIM: Dostarczono po dostawie produktow " & To_String(Assembly_Name(Assembly)) & " numer " &
                                Integer'Image(Assembly_Number(Assembly))& ESC & "[0m");
                     for W in Producer_Type loop
                        In_Storage := In_Storage - Lacking_assembly(W);
                        Storage(W) := Storage(W) - Lacking_assembly(W);
                     end loop;
                     Number := Assembly_Number(Assembly);
                     Assembly_Number(Assembly) := Assembly_Number(Assembly) + 1;
                  else
                     for W in Producer_Type loop
                        --Put_Line(ESC & "[91m" & " dodawanie " & Integer'Image(Assembly_Content(Assembly, W)) & " " & Integer'Image(Lacking_assembly(W)) & ESC & "[0m");
                        In_Storage := In_Storage + Assembly_Content(Assembly, W) - Lacking_assembly(W);
                        Storage(W) := Storage(W) + Assembly_Content(Assembly, W) - Lacking_assembly(W);
                     end loop;
                     --Put_Line( "dodawanie" );
                     --Storage_Contents;
                     Put_Line(ESC & "[96m" & "MIM: Brakuje produktow do skompletowania kebaba " & To_String(Assembly_Name(Assembly)) & ", dostawa nie dotarla na czas."& ESC & "[0m");
                     Number:=0;
                  end if;
               else
                  Put_Line(ESC & "[96m" & "MIM: Brakuje produktow do skompletowania kebaba " & To_String(Assembly_Name(Assembly)) & ", czekamy na dostawe." & ESC & "[0m");
                  Number := 0;
               end if;
            end Deliver;
         or
            accept Inspection_In_Storage do
               for W in Producer_Type loop
                  if Storage(W) > Storage_Capacity_for_single_item then
                     Put_Line(ESC & "[91m" & "INSPEKTOR: Za duzo " & To_String(Product_Name(W)) & ", trzeba wyrzucic nadmiar! " & ESC & "[0m");
                     In_Storage := In_Storage - (Storage(W)-Storage_Capacity_for_single_item);
                     Storage(W):= Storage(W) - (Storage(W)-Storage_Capacity_for_single_item);
                  end if;
                  Put_Line(ESC & "[96m" & "INSPEKTOR: Koniec Kontroli" & ESC & "[0m");
               end loop;
               end Inspection_In_Storage;
            end select;
            Storage_Contents;
      end loop;
   end Buffer;



   ---"MAIN" FOR SIMULATION---
begin
   I.Start;
   for I in 1 .. Number_Of_Producers loop
      P(I).Start(I, 10);
   end loop;
   for J in 1 .. Number_Of_Consumers loop
      K(J).Start(J,12);
   end loop;
end main;
