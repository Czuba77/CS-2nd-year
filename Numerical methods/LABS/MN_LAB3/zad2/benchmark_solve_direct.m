function [A,b,x,vec_time_direct] = benchmark_solve_direct(vN)
% Pomiar czasu rozwiązania length(vN) równań macierowych metodą LU,
% przy czym liczba zmiennych i-tego równania wynosi vN(i).
% A - tablica komórkowa zawierająca zestaw macierzy A do równania macierzowego
%       A{i}*x{i}=b{i}, gdzie size(A{i},1) = vN(i)
% b - tablica komórkowa prawych stron równań A{i}*x{i}=b{i}
% x - tablica komórkowa z rozwiązaniami równań A{i}*x{i}=b{i}
% vec_time_direct(i) - czas wyznaczenia i-tego rozwiązania metodą LU


x = [];
vec_time_direct = zeros(1,length(vN));

for i=1:length(vN)
    N = vN(i);

    [A{i},b{i}] = generate_matrix(N);
    tic
    % tu wyznacz x{i} metodą LU
    [A{i},b{i}] = generate_matrix(N);
    [L, U, P]=lu(A{i});
    y = L\(P*b{i});
    x{i} = U\y;
    vec_time_direct(i) = toc;
end
plot(vN, vec_time_direct)
xlabel('N')
ylabel('Czas')
title('Czas obliczen w zaleznosci od N')
print('zadanie2.png', '-dpng');
end