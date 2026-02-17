; Program przyk³adowy ilustruj¹cy operacje SSE procesora
; Poni¿szy podprogram jest przystosowany do wywo³ywania
; z poziomu jêzyka C (program arytmc_SSE.c)
.686
.XMM ; zezwolenie na asemblacjê rozkazów grupy SSE
.model flat

EXTRN _malloc:PROC
public _Matmul,_dodaj_SSE, _pierwiastek_SSE, _odwrotnosc_SSE, _int2float_SSE,_dodaj_char_SSE, _pm_jeden
.data
jedynki dd  1.0, 1.0, 1.0, 1.0
.code

_Matmul PROC
	push ebp
	mov ebp, esp
	sub esp, 8 ; -4 liczba wspolna -8 wielkosc macierzy
	push ebx
	push esi
	push edi

	mov esi, [ebp+8] ; adres A
	mov edi, [ebp+12] ; adres b
	mov ebx, [ebp+16] ; k


	mov eax, esi
	sub eax, edi ;szukanie roznicy adresow
	mov edx,0
	mov ecx, 8 
	div ecx
	dec eax ;odejmowanie bufora
	mov ecx, [ebp+20] ; m
	div ecx ; eax - liczba wspolna
	mov [ebp-4],eax ; zapisanie liczby wspolnej

	mov eax,ebx
	mul ecx
	mov [ebp-8],eax ; zapisanie wielkosci macierzy
	mov ecx, 4
	mul ecx ; mnozenie przez 4 zeby zaalokowac odpowiednia ilosc pamieci
	push eax
	call _malloc
	add esp,4	;eax adres obszaru macierzy wynikowej
	mov ecx,[ebp-4]
	mov edx,0

	finit
	mov ebx,0
ptzew:
	mov ecx,0
	fldz
pt1:
	push eax
	push edx
	mov edx,0
	mov eax, [ebp+20] 
	mul ecx		; m*ecx aby dostac sie do odpowiedniego wiersza
	fld qword ptr [edi+8*eax]
	pop edx
	pop eax
	fld qword ptr [esi+8*ecx]
	fmulp
	faddp
	inc ecx
	cmp ecx,[ebp-4]
	jne pt1
	inc ebx
	cmp ebx,[ebp+20]

	jne nadal_ten_wiersz
	push eax
	push edx
	mov eax,8
	mov edx,0
	mul dword ptr [ebp-4]

	add esi,eax ;przechodzenie do nastepnego wiersza w macierzy po lewej
	mov edi,[ebp+12]
	sub edi, 8
	mov ebx,0 

	pop edx
	pop eax
nadal_ten_wiersz:
	add edi, 8 ;przechodzenie do nastepnej kolumny w macierzy po prawej


	fstp dword ptr [eax+4*edx]
	inc edx
	cmp edx,[ebp-8]
	jne ptzew

	pop edi
	pop esi
	pop ebx
	add esp, 8
	pop ebp
	ret
_Matmul ENDP


_pm_jeden PROC
	push ebp
	mov ebp, esp
	push edi
	mov edi,[ebp+8]
	movups xmm5, [edi]
	mov ecx, offset jedynki
	movups xmm6, [ecx]
	addsubps xmm5, xmm6
	movups [edi], xmm5
	pop edi
	pop ebp
	ret
_pm_jeden ENDP

_int2float_SSE PROC
	PUSH EBP
	MOV EBP, ESP
	PUSH EBX
	PUSH ESI
	PUSH EDI
	mov esi, [ebp+8] ; adres pierwszej tablicy
	mov edi, [ebp+12] ; adres WYNIKOWEJ tablicy
	cvtpi2ps xmm5, qword PTR [esi]
	movups [edi], xmm5
	POP EDI
	POP ESI
	POP EBX
	POP EBP
	RET
_int2float_SSE ENDP

_dodaj_char_SSE PROC
	PUSH EBP
	MOV EBP, ESP
	PUSH EBX
	PUSH ESI
	PUSH EDI
	mov esi, [ebp+8] ; adres pierwszej tablicy
	mov edi, [ebp+12] ; adres drugiej tablicy
	mov ebx, [ebp+16] ; adres tablicy wynikowej
	movups xmm5, [esi]
	movups xmm6, [edi]
	paddsb xmm5, xmm6
	movups [ebx], xmm5
	POP EDI
	POP ESI
	POP EBX
	POP EBP
	ret
 _dodaj_char_SSE ENDP

_dodaj_SSE PROC
 push ebp
 mov ebp, esp
 push ebx
 push esi
 push edi
 mov esi, [ebp+8] ; adres pierwszej tablicy
 mov edi, [ebp+12] ; adres drugiej tablicy
 mov ebx, [ebp+16] ; adres tablicy wynikowej
; ³adowanie do rejestru xmm5 czterech liczb zmiennoprzecin-
; kowych 32-bitowych - liczby zostaj¹ pobrane z tablicy,
; której adres poczatkowy podany jest w rejestrze ESI
; interpretacja mnemonika "movups" :
; mov - operacja przes³ania,
; u - unaligned (adres obszaru nie jest podzielny przez 16),
; p - packed (do rejestru ³adowane s¹ od razu cztery liczby),
; s - short (inaczej float, liczby zmiennoprzecinkowe
; 32-bitowe)
 movups xmm5, [esi]
 movups xmm6, [edi]
; sumowanie czterech liczb zmiennoprzecinkowych zawartych
; w rejestrach xmm5 i xmm6
 addps xmm5, xmm6

; zapisanie wyniku sumowania w tablicy w pamiêci
 movups [ebx], xmm5
 pop edi
 pop esi
 pop ebx
 pop ebp
 ret
_dodaj_SSE ENDP
;=========================================================
_pierwiastek_SSE PROC
 push ebp
 mov ebp, esp
 push ebx
 push esi
 mov esi, [ebp+8] ; adres pierwszej tablicy
 mov ebx, [ebp+12] ; adres tablicy wynikowej
; ³adowanie do rejestru xmm5 czterech liczb zmiennoprzecin-
; kowych 32-bitowych - liczby zostaj¹ pobrane z tablicy,
; której adres pocz¹tkowy podany jest w rejestrze ESI
; mnemonik "movups": zob. komentarz podany w funkcji dodaj_SSE
 movups xmm6, [esi]
; obliczanie pierwiastka z czterech liczb zmiennoprzecinkowych
; znajduj¹cych sie w rejestrze xmm6
; - wynik wpisywany jest do xmm5
 sqrtps xmm5, xmm6

; zapisanie wyniku sumowania w tablicy w pamiêci
 movups [ebx], xmm5
 pop esi
 pop ebx
 pop ebp
 ret
_pierwiastek_SSE ENDP
;=========================================================
; rozkaz RCPPS wykonuje obliczenia na 12-bitowej mantysie
; (a nie na typowej 24-bitowej) - obliczenia wykonywane s¹
; szybciej, ale s¹ mniej dok³adne
_odwrotnosc_SSE PROC
 push ebp
 mov ebp, esp
 push ebx
 push esi
 mov esi, [ebp+8] ; adres pierwszej tablicy
 mov ebx, [ebp+12] ; adres tablicy wynikowej
; ladowanie do rejestru xmm5 czterech liczb zmiennoprzecin-
; kowych 32-bitowych - liczby zostaj¹ pobrane z tablicy,
; której adres poczatkowy podany jest w rejestrze ESI
; mnemonik "movups": zob. komentarz podany w funkcji dodaj_SSE
 movups xmm5, [esi]
; obliczanie odwrotnoœci czterech liczb zmiennoprzecinkowych
; znajduj¹cych siê w rejestrze xmm6
; - wynik wpisywany jest do xmm5
 rcpps xmm5, xmm6

; zapisanie wyniku sumowania w tablicy w pamieci
 movups [ebx], xmm5
 pop esi
 pop ebx
 pop ebp
 ret
_odwrotnosc_SSE ENDP
END
