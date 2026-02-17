#include <stdio.h> 
float* create_benford_distribution_asm();
int main() {
	//wchar_t data[] = L"3192480124, 3442, 341242134, 367654, 63442, 4233254, 52, 534, 533245, 325,"; 
	float* benford = create_benford_distribution_asm();
	return 0;
}