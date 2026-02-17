_plus_jeden PROC 
    push ebp 
    mov ebp, esp 
    push ebx 
    push esi 
    push edi 
    ; odczytanie liczby w formacie double 
    mov eax, [ebp+8] 
    mov edx, [ebp+12] 
    ; wpisanie 1 na pozycji o wadze 2^0 
    ; mantysy do EDI:ESI 
    mov esi, 0 
    mov edi, 00100000h 
    ; wyodrebnienie wykladnika (11 bitow) 
    mov ebx, edx 
    shr ebx, 20 
    ; obliczenie wykladnika 
    sub ebx, 1023 
    ; zerowanie wykladnika i bitu znaku 
    and edx, 000FFFFFh 
    ; dopisanie niejawnej 
    or edx, 00100000h 
    --- 
    cmp ebx,21
    jge zmien_bit_mlodszy
    push eax
    mov eax,20
    sub eax,ebx
    push ebx
    mov ebx,0
    bts ebx,eax
    add edx,ebx
    pop ebx
    btr edx,21
    jnc nie_zwiekszony_wykladnik
    inc ebx
    nie_zwiekszony_wykladnik:
    pop eax
    jmp koniec
zmien_bit_mlodszy:
    sub ebx,20
    push edx
    mov edx,31
    sub edx,ebx
    push esi
    mov esi,0
    bts esi,edx
    add eax,esi
    pop esi
    pop edx
    adc edx,0
    btr edx,21
    jnc nie_zwiekszony_wykladnik2
    inc ebx
    nie_zwiekszony_wykladnik2:
koniec:
	btr edx,20
    shl ebx, 20
    or edx, ebx
    mov ecx,[ebp+12]
    bt ecx,31
    jnc ustaw_zero
    bts edx,31
    jmp nie_zmien_bitu
    ustaw_zero:
    btr edx,31
    nie_zmien_bitu:
    push edx 
    push eax 
    fld qword ptr [esp] 
    add esp, 8 
    pop edi 
    pop esi 
    pop ebx 
    pop ebp 
    ret 
_plus_jeden ENDP