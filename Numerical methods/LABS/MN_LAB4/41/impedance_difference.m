function impedance_delta = impedance_difference(f)
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