#include <stdio.h>
#include <stdlib.h>
#include <sys/ipc.h>
#include <sys/msg.h>
#include <unistd.h>
#include <sys/wait.h>

#define SEM_KEY 2115
#define PERM 0666

#define PODKANAL_WEJSCIE 1
#define PODKANAL_WAIT 2

struct msgbuf {
    long podkanal;
    char znak;
};

void wejscie(int kanal) {
    struct msgbuf msg;
    msgrcv(kanal, &msg, sizeof(msg.znak), PODKANAL_WEJSCIE, 0);
}

void wyjscie(int kanal) {
    struct msgbuf msg;
    msg.podkanal = PODKANAL_WEJSCIE;
    msg.znak = 0;
    msgsnd(kanal, &msg, sizeof(msg.znak), IPC_NOWAIT);
}

void sem_wait(int kanal) {
    wyjscie(kanal);
    struct msgbuf msg;
    msgrcv(kanal, &msg, sizeof(msg.znak), PODKANAL_WAIT, 0);
    wejscie(kanal);
}

void sem_notify(int kanal) {
    struct msgbuf msg;
    msg.podkanal = PODKANAL_WAIT;
    msg.znak = 0;
    msgsnd(kanal, &msg, sizeof(msg.znak), IPC_NOWAIT);
}

void sem_init(int kanal) {
    struct msgbuf msg;
    msg.podkanal = PODKANAL_WEJSCIE;
    msg.znak = 0;
    msgsnd(kanal, &msg, sizeof(msg.znak), 0);
}

void monitor(int i, int kanal){
    wejscie(kanal);
    printf("Proces %d: wszedł do monitora\n", i);
    sleep(i);
    if (i % 2 != 0) {
        printf("Proces %d: czeka na warunek (wait)\n", i);
        sem_wait(kanal);
        printf("Proces %d: obudzony z wait\n", i);
    } else {
        printf("Proces %d: wykonuje notify\n", i);
        sem_notify(kanal);
    }

    printf("Proces %d: kończy pracę w monitorze\n", i);
    wyjscie(kanal);
    exit(0);
}

int main() {
    int kanal = msgget(SEM_KEY, IPC_CREAT | PERM);
    if (kanal == -1) {
        perror("msgget");
        exit(1);
    }

    sem_init(kanal);

    for (int i = 1; i <= 4; i++) {
        pid_t pid = fork();
        if (pid == 0) {
            monitor(i,kanal);
        }
    }

    for (int i = 0; i < 4; i++) {
        wait(NULL);
    }

    msgctl(kanal, IPC_RMID, NULL);
    printf("Monitor zakończył działanie\n");

    return 0;
}
