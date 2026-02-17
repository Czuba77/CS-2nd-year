.686
.model flat
extern _ExitProcess@4 : PROC
extern __read : PROC
extern __write : PROC
public _srednia_harm
public _nowy_exp
.data

jeden dd 1.0
osiem dd 8.0
piec dd 5.0


.code
_srednia_harm PROC
	push ebp
	mov ebp, esp
	mov ecx, [ebp+12]
	mov edx, [ebp+8]
	lea edx, [edx]

	finit
	fld dword ptr [jeden]
	fld dword ptr [edx+4*ecx-4]
	fdivp ;dzielenie 1/x
	dec ecx
l2:
	fld dword ptr [jeden]
	fld dword ptr [edx+4*ecx-4]
	fdivp ;dzielenie 1/x
	faddp 
	loop l2
	fild dword ptr [ebp+12] ;zaladowanie n
	fxch
	fdivp ;podzielenie przez n

	pop ebp
	ret
_srednia_harm ENDP

_nowy_exp PROC
	push ebp
	mov ebp, esp
	push esi
	push edi

	mov esi, 1
	finit


	fld dword ptr [jeden] ;1
	fld dword ptr [ebp+8]
	fld dword ptr [jeden] ;x/2
	fdivp
	faddp
	fld dword ptr [jeden]
ptne1:
	add esi, 1
	mov ecx, esi
	fadd dword ptr [jeden]
	fst st(2)
	fxch
	fld dword ptr [jeden]
	fxch
	fxch st(4)
	fstp st(0) ;st1 obecny moment silni 
				;st2 maks silnia obecna
				;st3 laczna suma
ptne2:
	fmul st(0), st(1)
	cmp ecx,2	
	je czykoniec
	fxch
	fsub dword ptr [jeden]
	fxch
	dec ecx
	jmp ptne2

czykoniec:
	fxch ;usuwanie tymczasowej silni
	fstp st(0)
	fld dword ptr [ebp+8] ;zaladownia x
	mov edi,esi
ptne3:
	fld dword ptr [ebp+8]
	fmulp ;obliczanie potegi
	dec edi
	cmp edi,1
	jne ptne3
	fxch
	fdivp ;podzielenie przez sume	
	fxch
	fxch st(2)
	faddp
	fxch
	cmp esi, 20
	jne ptne1

	fstp st(0)
	fxch 
	fstp st(0)
	fxch
	fstp st(0)
	fxch
	fstp st(0)
	fxch
	fstp st(0)
	fxch
	fstp st(0)
	fxch
	fstp st(0)
	fxch
	fstp st(0)
	pop edi
	pop esi
	pop ebp
	ret
_nowy_exp ENDP

END