/* Program przyk³adowy ilustruj¹cy operacje SSE procesora
 Program jest przystosowany do wspó³pracy z podprogramem
 zakodowanym w asemblerze (plik arytm_SSE.asm)
*/


#include <stdio.h>
void dodaj_SSE(float*, float*, float*);
void dodaj_char_SSE(char*, char*, char*);
void pierwiastek_SSE(float*, float*);
void odwrotnosc_SSE(float*, float*);
void int2float_SSE(int* calkowite, float* zmienno_przec);
void pm_jeden(float* tabl);
float* Matmul(double* A, double* B, unsigned int k, unsigned int m);

int main()
{
	double A[6] = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
	double B[12] = { 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 7.0, 8.0, 9.0};
	


	float* C = Matmul((double*)A, (double*)B, 2, 4);
	printf("\n\nMnozenie macierzy\n");
	printf("\n%f %f %f %f %f %f %f %f", C[0], C[1], C[2], C[3], C[4], C[5], C[6], C[7]);
	free(C);
	/*
	float tablica[4] = { 27.5,143.57,2100.0, -3.51 };
	printf("\n%f %f %f %f\n", tablica[0],tablica[1], tablica[2], tablica[3]);
	pm_jeden(tablica);
	printf("\n%f %f %f %f\n", tablica[0],tablica[1], tablica[2], tablica[3]);

	int calkowite[2] = { 12, -17};
	float zmienno_przec[4];
	int2float_SSE(calkowite, zmienno_przec);
	printf("\n\nKonwersja liczb calkowitych na zmienno przecinkowe\n");
	printf("\n%d %d", calkowite[0], calkowite[1]);
	printf("\n%f %f", zmienno_przec[0], zmienno_przec[1]);

	char liczby_A[16] = { -128, -127, -126, -125, -124, -123, -122,-121, 120, 121, 122, 123, 124, 125, 126, 127 };
	char liczby_B[16] = { -3, -3, -3, -3, -3, -3, -3, -3,3, 3, 3, 3, 3, 3, 3, 3 };
	char wynik[16];
	dodaj_char_SSE(liczby_A, liczby_B, wynik);
	printf("\n\nDodawanie liczb calkowitych\n");
	printf("\n%d %d %d %d %d %d %d %d %d %d %d %d %d %d %d %d", liczby_A[0], liczby_A[1], liczby_A[2], liczby_A[3], liczby_A[4], liczby_A[5], liczby_A[6], liczby_A[7], liczby_A[8], liczby_A[9], liczby_A[10], liczby_A[11], liczby_A[12], liczby_A[13], liczby_A[14], liczby_A[15]);
	printf("\n%d %d %d %d %d %d %d %d %d %d %d %d %d %d %d %d", liczby_B[0], liczby_B[1], liczby_B[2], liczby_B[3], liczby_B[4], liczby_B[5], liczby_B[6], liczby_B[7], liczby_B[8], liczby_B[9], liczby_B[10], liczby_B[11], liczby_B[12], liczby_B[13], liczby_B[14], liczby_B[15]);
	printf("\n%d %d %d %d %d %d %d %d %d %d %d %d %d %d %d %d", wynik[0], wynik[1], wynik[2], wynik[3], wynik[4], wynik[5], wynik[6], wynik[7], wynik[8], wynik[9], wynik[10], wynik[11], wynik[12], wynik[13], wynik[14], wynik[15]);
	
	float p[4] = { 1.0, 1.5, 2.0, 2.5 };
	float q[4] = { 0.25, -0.5, 1.0, -1.75 };
	float r[4];
	dodaj_SSE(p, q, r);
	printf("\n%f %f %f %f", p[0], p[1], p[2], p[3]);
	printf("\n%f %f %f %f", q[0], q[1], q[2], q[3]);
	printf("\n%f %f %f %f", r[0], r[1], r[2], r[3]);
	printf("\n\nObliczanie pierwiastka");
	pierwiastek_SSE(p, r);
	printf("\n%f %f %f %f", p[0], p[1], p[2], p[3]);
	printf("\n%f %f %f %f", r[0], r[1], r[2], r[3]);
	printf("\n\nObliczanie odwrotnosci - ze wzglêdu na stosowanie");
	printf("\n12-bitowej mantysy obliczenia sa malo dokladne");
	odwrotnosc_SSE(p, r);
	printf("\n%f %f %f %f", p[0], p[1], p[2], p[3]);
	printf("\n%f %f %f %f", r[0], r[1], r[2], r[3]);
	*/
	return 0;
}

