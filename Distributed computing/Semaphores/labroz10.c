#include <stdio.h>
#include <stdlib.h>
#include <sys/ipc.h>
#include <sys/msg.h>
#include <unistd.h>

#define SEM_KEY 2115
#define PERM 0666
#define PODKANAL 1

struct m_komunikat {
    long podkanal ;
    char znak;
};



void sem_wait(int KANAL){
    struct m_komunikat kom;
    msgrcv(KANAL,(struct msgbuf *)&kom, sizeof(kom) - sizeof(long), PODKANAL,0);
}

void sem_raise(int KANAL){
    struct m_komunikat kom;
    kom.podkanal = PODKANAL;
    kom.znak = 'a';
    msgsnd(KANAL, (struct msgbuf *)&kom, sizeof(kom) - sizeof(long), IPC_NOWAIT);
}

void sem_init(int KANAL){
    sem_raise(KANAL);
}
int main(){
    int KANAL = msgget(SEM_KEY, IPC_CREAT | PERM);
    if (KANAL == -1) {
        perror("msgget");
        exit(1);
    }
    sem_init(KANAL);
    if(fork()){
        sem_wait(KANAL);
        printf("Proces 1: Blokuje\n");
        sleep(8);
        sem_raise(KANAL);
        printf("Proces 1: Zwalniam\n");
    }
    else{
        sleep(5);
        printf("Proces 2: Czekam\n");
        sem_wait(KANAL);
        printf("Proces 2: Odebralem\n");
    }
    return 0;
}

