function [r_norm] = solve_Gauss_Seidel(A,b)
% A - macierz z równania macierzowego A * x = b
% b - wektor prawej strony równania macierzowego A * x = b
% U - macierz trójkątna górna, która zawiera wszystkie elementy macierzy A powyżej głównej diagonalnej,
% T - macierz trójkątna dolna równa A-U
% w - wektor pomocniczy opisany w instrukcji do Laboratorium 3
%       – sprawdź wzór (7) w instrukcji, który definiuje w jako w_{GS}.
% x - rozwiązanie równania macierzowego
% r_norm - wektor norm residuum kolejnych przybliżeń rozwiązania; norm(A*x-b);
% iteration_count - liczba iteracji wymagana do wyznaczenia rozwiązania
%       metodą Gaussa-Seidla
iteration_count = 0;
N=length(b);

D=diag(diag(A));
U=triu(A,1);
L=tril(A,-1);
T=(A-U);

x = ones(N,1);
r_norm=norm(A*(ones(N,1))-b);


inorm = norm(A*x-b);
while(inorm>1e-12 && iteration_count<1000)
    w=(D+L)\b;
    x = -T\(U*x)+w;
    inorm = norm(A*x - b);
    iteration_count = iteration_count+1;
    r_norm = [r_norm, inorm];
end
end