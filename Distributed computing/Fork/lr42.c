#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <math.h>
#include <sys/types.h>
#include <unistd.h>
#include <sys/wait.h>

#define NUMBER_OF_SECTIONS 7

void leaf_func(int sec);
int SECTION[NUMBER_OF_SECTIONS+1];
const int MAX = 1000000;
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

int calculate(int beg, int end){
    int count = 0;
    for (int i = beg; i < end; i++) {
        if (is_prime(i) == 1 && is_fibonacci(i) == 1) {
            printf("%d \n", i);
            count++;
        }
    }
    return count;
}

void leaf_func(int sec){
    int count = calculate(SECTION[sec],SECTION[sec+1]);
    exit(count);
}

void child(int sec){
    int count=0;
    pid_t p = fork();
    if(p<0){
      perror("fork fail");
      exit(0);
    }
    else if(p==0){//leaf
        leaf_func(sec+1);
    }
    else{//ojceic kid
        pid_t p2 = fork();
        if(p2<0){
            perror("fork fail");
            exit(0);
        }
        else if(p2==0){//leaf
            leaf_func(sec+2);
        }
        else{//ojceic kid

            count += calculate(SECTION[sec],SECTION[sec+1]);
            int wstatus;
            waitpid(p, &wstatus, 0);
            int primes_found_by_child_process = WEXITSTATUS(wstatus);
            count += primes_found_by_child_process;
            waitpid(p2, &wstatus, 0);
            primes_found_by_child_process = WEXITSTATUS(wstatus);
            count += primes_found_by_child_process;
            exit(count);
        }
    }
}



int main()
{
    for(int i=0; i<NUMBER_OF_SECTIONS+1;i++){
        SECTION[i] = (MAX/NUMBER_OF_SECTIONS)*i;
    }
    SECTION[0]+=2;
    clock_t t;
    t = clock();
    int count=0;
    int sec = 1; //0 dla maina
    pid_t p = fork();
    if(p<0){
      perror("fork fail");
      exit(0);
    }
    else if(p==0){//kid
        child(sec);
    }
    else{//ojceic
        pid_t p2 = fork();
        if(p2<0){
            perror("fork fail");
            exit(0);
        }
        else if(p2==0){//kid
            child(sec+3);
        }
        else{//ojceic
            count += calculate(SECTION[0],SECTION[sec]);
            int wstatus;
            waitpid(p, &wstatus, 0);
            int primes_found_by_child_process = WEXITSTATUS(wstatus);
            count += primes_found_by_child_process;
            waitpid(p2, &wstatus, 0);
            primes_found_by_child_process = WEXITSTATUS(wstatus);
            count += primes_found_by_child_process;
        }
    }
    t = clock() - t;
    double time_taken = ((double)t) / CLOCKS_PER_SEC; // in seconds 
    printf("for took %f seconds to execute \n", time_taken);
    printf("number of numbers %d \n", count);
}