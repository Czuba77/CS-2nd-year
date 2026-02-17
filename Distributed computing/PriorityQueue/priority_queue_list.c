#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "priority_queue_list.h"


void qlist(pqueue *head, void (*print_data)(void *)) {
	pqueue *p;
	
	for (p = head; p != NULL; p = p->next) {
		printf("%d: ", p->k);
		print_data(p->data);
		printf("\n");
	}
	
}

void qinsert(pqueue **phead, void *data, int k) {
	if ((* phead) == NULL){
		(*phead) = (struct pqueue*)malloc(sizeof(struct pqueue));
		(*phead)->next = NULL;
		(*phead)->prev = NULL;
		(*phead)->data = NULL;
	}
	struct pqueue* p=(*phead)->next;
	if (p != NULL) {
		if (k > (p->k)) {
			qinsert(&p, data,k);
		}
		else {
			p->prev = (struct pqueue*)malloc(sizeof(struct pqueue));
			p = p->prev;
			p->k = k;
			p->data = data;
			p->next = (*phead)->next;
			p->prev = (*phead);
			(*phead)->next = p;
		}
	}
	else {
		p= (struct pqueue*)malloc(sizeof(struct pqueue));
		p->k = k;
		p->data = data;
		if ((*phead)->data != NULL) {
			if (k > (*phead)->k) {
				p->next = NULL;
				p->prev = (*phead);
				(*phead)->next = p;
			}
			else {
				p->prev = NULL;
				p->next = (*phead);
				(*phead)->prev = p;
				(*phead) = p;
			}
		}
		else {
			p->next = NULL;
			p->prev = NULL;
			free(*phead);
			(*phead) = p;
		}
		
	}

}


void qremove(pqueue **phead, int k) {
	if ((*phead)->next != NULL) {
		if ((*phead)->prev == NULL && (*phead)->k == k) {
			struct pqueue* p = (*phead);
			(*phead) = (*phead)->next;
			(*phead)->prev = NULL;
			free(p);
		}
		else if ((*phead)->k == k) { //znaleziono wezel
			struct pqueue* p = (*phead);
			(*phead) = (*phead)->prev;
			(*phead)->next = p->next;
			(p->next)->prev = p->prev;
			free(p);
		}
		else {//jedz dalej
			qremove(&((*phead)->next),k);
		}
	}
	else {
		if ((*phead)->k == k) {
			struct pqueue* p = (*phead);
			(*phead) = (*phead)->prev;
			(*phead)->next = NULL;
			free(p);
		}
		else {
			printf("Zle k");
		}
	}
}

