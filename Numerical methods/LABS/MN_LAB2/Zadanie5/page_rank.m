function [index_number, Edges, I, B, A, b, r] = page_rank()
index_number=201096;
L=mod(index_number,10);
wezel=(1+mod(L,7));
N=7;
d=0.85;
Edges = [1,1,2,2,2,3,3,3,4,4,5,5,6,6,7;
         4,6,3,4,5,5,6,7,5,6,4,6,4,7,6];

I = speye(N,N);
B = sparse(Edges(2,:),Edges(1,:),1,N,N);
for i=1:N
    if B(wezel,i) == 0 && i ~= wezel
        Edges(:,end+1)=[i,wezel];
    end
    if B(i,wezel) == 0 && i ~= wezel
        
        Edges(:,end+1)=[wezel,i];
    end
end
B = sparse(Edges(2,:),Edges(1,:),1,N,N);

Larr=zeros(N,1);
for i =1:7
    Larr(i)=1/sum(B(:,i));
end
A = spdiags(Larr,0,N,N);
b = zeros(N,1);
for i=1:N
    b(i)=(1-d)/N;
end
M=I-d*B*A;
r = M^-1*b;
hold on;
bar(r);
title("Page rank");
xlabel("Numer strony");
ylabel("Ilosc odnosnikow");
hold off;
print -dpng zadanie5.png
end