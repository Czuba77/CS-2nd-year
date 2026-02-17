#include <stdio.h>

unsigned int read2msg(char* filename);


int main() {
	char filename[] = "msg.txt";
	unsigned int msg = read2msg(filename);
	return 0;
}