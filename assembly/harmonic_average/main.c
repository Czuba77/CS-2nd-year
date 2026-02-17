#include <stdio.h>

float srednia_harm(float* tablica, unsigned int n);
float nowy_exp(float x);



void main() {
	float tablica[] = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0,8.0,9.0 };
	unsigned int n = 9;
	float wynik = srednia_harm(tablica, n);
	printf("Srednia harmoniczna: %f\n", wynik);
	float x = 2.0;
	float wynik2 = nowy_exp(x);
	printf("Nowy exp: %f\n", wynik2);
}