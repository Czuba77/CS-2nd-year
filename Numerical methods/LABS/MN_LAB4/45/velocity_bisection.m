function [xvec,xdif,xsolution,ysolution,iterations] = velocity_bisection()
% Wyznacza miejsce zerowe funkcji velocity_difference metodą bisekcji.
% xvec - wektor z kolejnymi przybliżeniami miejsca zerowego, gdzie xvec(1)= (a+b)/2
% xdif - wektor różnic kolejnych przybliżeń miejsca zerowego
%   xdif(i) = abs(xvec(i+1)-xvec(i));
% xsolution - obliczone miejsce zerowe
% ysolution - wartość funkcji velocity_difference wyznaczona dla t=xsolution
% iterations - liczba iteracji wykonana w celu wyznaczenia xsolution

a = 1; % lewa granica przedziału poszukiwań miejsca zerowego
b = 40; % prawa granica przedziału poszukiwań miejsca zerowego
ytolerance = 1e-12; % tolerancja wartości funkcji w przybliżonym miejscu zerowym.
% Warunek abs(f1(xsolution))<ytolerance określa jak blisko zera ma znaleźć
% się wartość funkcji w obliczonym miejscu zerowym funkcji f1(), aby obliczenia
% zostały zakończone.
max_iterations = 1000; % maksymalna liczba iteracji wykonana przez alg. bisekcji

fa = velocity_difference(a);
fb = velocity_difference(b);

xvec = [];
xdif = [];
xsolution = Inf;
ysolution = Inf;
iterations = max_iterations;

for ii=1:max_iterations
     c = (a+b)/2;
    xvec(ii,1) = c;
    if ii ~= 1
        xdif(ii) = abs(xvec(ii) - xvec(ii-1));
    end
    fc = velocity_difference(c);
    if(abs(fc)<ytolerance)
        xsolution = c;
        ysolution = fc;
        iterations = ii;
        break
    elseif fa*fc<0
        b=c;
        fb=fc;
    else
        a=c;
        fa=fc;
    end
end

    xdif = abs(diff(xvec));
    figure;

    subplot(2,1,1);
    plot(xvec, '-');
    title('Przybliżenia miejsca zerowego');
    xlabel('Iteracja');
    ylabel('Czas');
    grid on;

    subplot(2,1,2);
    semilogy(xdif, '-');
    title('Różnica między kolejnymi przybliżeniami (log)');
    xlabel('Iteracja');
    ylabel('|x(i) - x(i-1)|');
    grid on;
    saveas(gcf, 'zadanie5.png');
end


function velocity_delta = velocity_difference(t)
% Tutaj wklej definicję funkcji zdefiniowanej w zadaniu 4.
if t <= 0
    error('czas musi być większy od zera.');
end
u=2000;
m0=150000;
q=2700;
g=1.622
M=700;
vt=u*log(m0/(m0-q*t))-g*t;
velocity_delta = vt-M;


end