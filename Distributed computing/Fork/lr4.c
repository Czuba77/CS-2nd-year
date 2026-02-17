// labrozprochy4.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <stdio.h>
#include <time.h>
#include <math.h>

int is_prime(int x) {
    double y = (double)x;
    for (int i = 2; i < sqrt(y); i++){
        if (x % i == 0) {
            return 0;
        }
    }
    return 1;
}

int is_fibonacci(int x) {
    int a=1, b=1,c=1;
    if (c == x) {
        return 1;
    }
    while (a < x) {
        c = a + b;
        if (c == x) {
            return 1;
        }
        b = a;
        a = c;
    }
    return 0;
}

int main()
{
    clock_t t;
    t = clock();

    int count = 1;
    for (int i = 3; i < 10000000; i+=2) {
        if (is_prime(i) == 1 && is_fibonacci(i) == 1) {
            printf("%d \n", i);
            count++;
        }
    }
    t = clock() - t;
    double time_taken = ((double)t) / CLOCKS_PER_SEC; // in seconds 
    printf("for took %f seconds to execute \n", time_taken);
    printf("number of numbers %d \n", count);
}