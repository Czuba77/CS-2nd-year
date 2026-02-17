.686
.model flat
extern __write : PROC
extern __read : PROC
extern _ExitProcess@4 : PROC
public _main
.data
znaki db 12 dup (?)
obszar db 12 dup (?)
dziesiec dd 10 ; mno¿nik
.code
wyswietl_EAX PROC
pusha

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
sub cl, 30H ; zamiana kodu ASCII na wartoœæ cyfry
movzx ecx, cl ; przechowanie wartoœci cyfry w
; rejestrze ECX
; mno¿enie wczeœniej obliczonej wartoœci razy 10
mul dword PTR dziesiec
cmp EDX,0
jg edxwieksze
add eax, ecx ; dodanie ostatnio odczytanej cyfry
jc edxwieksze
jmp pobieraj_znaki ; skok na pocz¹tek pêtli
edxwieksze:
mov eax,0
byl_enter:
; wartoœæ binarna wprowadzonej liczby znajduje siê teraz rejestrze EAX



pop ECX
pop EBX
ret
wczytaj_EAX ENDP


_main PROC

call wczytaj_EAX

push 0
call _ExitProcess@4
_main ENDP
END