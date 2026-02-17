#include <unistd.h>
#include <stdio.h>
#include <ctype.h>
#include <sys/wait.h>
#include <fcntl.h>
#include <stdlib.h>
#include <string.h>
#include <sys/stat.h>

#define ODCZYT 0
#define ZAPIS 1
#define WIELKOSCTAB 10
int main() {
    mkfifo("pipe", 0666);
    mkfifo("pipe2", 0666);
    char txt[WIELKOSCTAB] = {0};
    int f=0;
    int pid=fork();
    if (pid>0) {
        scanf("%s", txt);
        f = open("pipe", O_WRONLY);
        write(f,txt,sizeof(char)*WIELKOSCTAB);
        close(f);
        puts("Koniec procesu 1");
    } else {
        int pid2=fork();
        if (pid2>0) {
            f = open("pipe", O_RDONLY);
            read(f,txt,sizeof(char)*WIELKOSCTAB);
            for(int i=0;i<WIELKOSCTAB;i++)
                txt[i]=toupper(txt[i]);
            close(f);
            f = open("pipe2", O_WRONLY); 
            write(f,txt,sizeof(char)*WIELKOSCTAB);
            close(f);
            puts("Koniec procesu 2");
        } else {
            f = open("pipe2", O_RDONLY);   
            read(f,txt,sizeof(char)*WIELKOSCTAB);
            close(f);
            printf("%s\n",txt);
            puts("Koniec procesu 3");
        }
    }
    return 0;
}
