function [xvec,xdif,xsolution,ysolution,iterations] = impedance_bisection()


a = 1; % lewa granica przedziału poszukiwań miejsca zerowego
b = 10; % prawa granica przedziału poszukiwań miejsca zerowego
ytolerance = 1e-12; % tolerancja wartości funkcji w przybliżonym miejscu zerowym.
% Warunek abs(f1(xsolution))<ytolerance określa jak blisko zera ma znaleźć
% się wartość funkcji w obliczonym miejscu zerowym funkcji f1(), aby obliczenia
% zostały zakończone.
max_iterations = 1000; % maksymalna liczba iteracji wykonana przez alg. bisekcji

fa = impedance_difference(a);
fb = impedance_difference(b);

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
    fc = impedance_difference(c);
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
    ylabel('Częstotliwość (Hz)');
    grid on;

    subplot(2,1,2);
    semilogy(xdif, '-');
    title('Różnica między kolejnymi przybliżeniami (log)');
    xlabel('Iteracja');
    ylabel('|x(i) - x(i-1)|');
    grid on;
    saveas(gcf, 'zadanie2.png');
end

function impedance_delta = impedance_difference (f)
if f <= 0
    error('Częstotliwość musi być większa od zera.');
end
impedance_delta = 0;
R = 525;
C = 7 *10^(-5);
L = 3;
M = 75;

function_val = 1/(sqrt(1/(R^2) + (2*pi*f*C -1/(2*pi*f*L))^2));
impedance_delta = abs(function_val)-M;

end