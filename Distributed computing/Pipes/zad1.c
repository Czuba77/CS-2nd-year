#include <unistd.h>
#include <stdio.h>
#include <ctype.h>
#include <sys/wait.h>
#define ODCZYT 0
#define ZAPIS 1
#define WIELKOSCTAB 10
int main() {
    int potok[2];
    int potok2[2];
    int x;
    char txt[WIELKOSCTAB] = {0};

    pipe(potok);
    pipe(potok2);
    int pid=fork();
    if (pid>0) {
        close(potok[ODCZYT]);
        close(potok2[ZAPIS]);
        close(potok2[ODCZYT]);
        scanf("%s", txt);
        write(potok[ZAPIS],txt,sizeof(char)*WIELKOSCTAB);
        close(potok[ZAPIS]);
        puts("Koniec procesu 1");
    } else {
        int pid2=fork();
        if (pid2>0) {
            close(potok[ZAPIS]);
            close(potok2[ODCZYT]);
            read(potok[ODCZYT],txt,sizeof(char)*WIELKOSCTAB);
            for(int i=0;i<WIELKOSCTAB;i++)
                txt[i]=toupper(txt[i]);
            close(potok[ODCZYT]);
            write(potok2[ZAPIS],txt,sizeof(char)*WIELKOSCTAB);
            close(potok2[ZAPIS]);
            puts("Koniec procesu 2");
        } else {
            close(potok[ODCZYT]);
            close(potok[ZAPIS]);
            close(potok2[ZAPIS]);
            read(potok2[ODCZYT],txt,sizeof(char)*WIELKOSCTAB);
            close(potok2[ODCZYT]);
            printf("%s\n",txt);
            puts("Koniec procesu 3");
        }
    }
    return 0;
}
