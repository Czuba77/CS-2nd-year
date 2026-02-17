.686
.model flat
extern __write : PROC
extern __read : PROC
extern _ExitProcess@4 : PROC
public _main
.data
znaki db 12 dup (?)
obszar db 12 dup (?)
dwanascie dd 12 ; mno¿nik
.code
wyswietl_EAX PROC
pusha
	mov edx, 0
	mov ebx, 10 ; dzielna
	mov edi, 0 ;rej do ilosci znakow
	mov ecx, 0 ; rej do porawnego zapisu w pamieci
l2:
	mov edx, 0
	div  ebx
	add edx, 30h ; zmiana reszty na dobry kod ascii
	push edx ;reszta dzielenia
	add edi,1
	cmp eax, 0 
	jne l2
	
	add edi,1
	mov esi, edi ;przygotowanie miejsca na dopisanie ,
	sub esi,4
l3:
	pop edx
	mov znaki[ecx],dl
	add ecx,1
	cmp ecx, esi
	jne et1
	mov znaki[ecx],',' 
	add ecx,1
	et1:
	cmp edi,ecx 
	jne l3

	mov znaki[ecx],0ah ;new line
	add edi,1

	push edi ;ile znakow
	push dword PTR offset znaki ;adres
	push dword PTR 1;uchwyt do ekranu
	call __write
	add esp, 12

popa
ret
wyswietl_EAX ENDP


wczytaj_EAX PROC
push EBX
push ECX

; wczytywanie liczby dziesiêtnej z klawiatury – po
; wprowadzeniu cyfr nale¿y nacisn¹æ klawisz Enter
; liczba po konwersji na postaæ binarn¹ zostaje wpisana
; do rejestru EAX
; deklaracja tablicy do przechowywania wprowadzanych cyfr
; (w obszarze danych)
; max iloœæ znaków wczytywanej liczby
push dword PTR 12
push dword PTR OFFSET obszar ; adres obszaru pamiêci
push dword PTR 0; numer urz¹dzenia (0 dla klawiatury)
call __read ; odczytywanie znaków z klawiatury
; (dwa znaki podkreœlenia przed read)
add esp, 12 ; usuniêcie parametrów ze stosu
; bie¿¹ca wartoœæ przekszta³canej liczby przechowywana jest
; w rejestrze EAX; przyjmujemy 0 jako wartoœæ pocz¹tkow¹
mov edx,0
mov eax, 0
mov ebx, OFFSET obszar ; adres obszaru ze znakami
pobieraj_znaki:
mov cl, [ebx] ; pobranie kolejnej cyfry w kodzie
; ASCII
inc ebx ; zwiêkszenie indeksu
cmp cl,10 ; sprawdzenie czy naciœniêto Enter
je byl_enter ; skok, gdy naciœniêto Enter
cmp cl,'A'
jne et1
mov cl, 10
jmp byloA_B
et1:
cmp cl,'B'
jne et2
mov cl, 11
jmp byloA_B
et2:
cmp cl,'a'
jne et3
mov cl, 10
jmp byloA_B
et3:
cmp cl,'b'
jne niebyloA_B
mov cl, 11
jmp byloA_B

niebyloA_B:
sub cl, 30H ; zamiana kodu ASCII na wartoœæ cyfry
byloA_B:
movzx ecx, cl ; przechowanie wartoœci cyfry w
; rejestrze ECX
; mno¿enie wczeœniej obliczonej wartoœci razy 10
mul dword PTR dwanascie
cmp EDX,0
add eax, ecx ; dodanie ostatnio odczytanej cyfry
jmp pobieraj_znaki ; skok na pocz¹tek pêtli
byl_enter:
; wartoœæ binarna wprowadzonej liczby znajduje siê teraz rejestrze EAX



pop ECX
pop EBX
ret
wczytaj_EAX ENDP

_main PROC

call wczytaj_EAX
mov ecx,eax
call wczytaj_EAX

mov edi, 1000

mov ebx,eax
mov eax,ecx
mul edi
div ebx

call wyswietl_EAX
;call wczytaj_EAX_hex
push 0
call _ExitProcess@4
_main ENDP
END