 .686 
.model flat 
public _create_benford_distribution_asm 
extern _VirtualAlloc@16 : proc ; to kurwa nie dziala 
extern _malloc : proc 
.code 
_create_benford_distribution_asm PROC 
push ebp 
mov ebp, esp 
push edi 
push ebx 
push esi
push dword ptr 36
call _malloc
add esp, 4
mov edi, eax

finit
mov ecx,1
_L1:
push ecx
fild dword ptr [esp]
add esp, 4
fld1
fxch
fdiv
fld1
fadd ;(1+1/k)
fld1
fxch
fyl2x
push dword ptr 10
fild dword ptr [esp]
add esp, 4
fld1
fxch
fyl2x
fdiv
fistp dword ptr [edi+ecx*4-4]
add ecx,1
cmp ecx, 10
jne _L1

pop esi
pop ebx 
pop edi 
pop ebp 
ret 
_create_benford_distribution_asm ENDP 
END