.686
.model flat
public _read2msg
extern _fopen : PROC
extern _fread : PROC
extern _fclose : PROC
extern _MessageBoxW@16 : PROC
.data
mial dw 'r',0
tekst dw 512 dup(0)

.code

_read2msg PROC
	push ebp
	mov ebp, esp
	sub esp, 1032;miejsce na zmienne lokalne
	push esi 
	push edi
	push ebx


	
	mov [ebp - 4], dword ptr 'r' ;tryb otwarcia pliku
	lea ebx, [ebp - 4]

	push ebx
	push [ebp + 8] ;adres nazwy pliku
	call _fopen
	add esp, 8

	test eax, eax                        ; Check if eax (return value) is NULL
    jz fopen_failed  

	lea ecx, [ebp - 1032]

	mov ebx, eax ;uchwyt pliku
	push ebx
	push dword ptr 512
	push dword ptr 2
	push ecx
	call _fread
	add esp, 16
	lea edi, [ebp - 1032 + eax*2]
	mov [edi], dword ptr 0

	lea ecx, [ebp - 1032]
	push dword ptr 0
	push ecx
	push ecx
	push dword ptr 0
	call _MessageBoxW@16

fopen_failed:
	pop ebx
	pop edi
	pop esi
	add esp, 1032
	pop ebp
	ret
_read2msg ENDP
		END