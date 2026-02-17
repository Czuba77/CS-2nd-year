function [x, y, z, zmin, lake_volume] = compute_lake_volume_monte_carlo()
    % Wyznacza objętość jeziora metodą Monte Carlo.
    %
    % x/y/z - wektory wierszowe, które zawierają współrzędne x/y/z punktów
    %       wylosowanych w celu wyznaczenia przybliżonej objętości jeziora
    % zmin - minimalna dopuszczalna wartość współrzędnej z losowanych punktów
    % lake_volume - objętość jeziora wyznaczona metodą Monte Carlo
    N = 1e6;
    x = 100 * rand(1, N); % [m]
    y = 100 * rand(1, N); % [m]

    zmin = -55 * rand(1) - 45;
    z = zmin + abs(zmin) * rand(1, N); 

    V = 100 * 100 * abs(zmin);

    lake_volume = monte_carlo_integral(N, x, y, z, V);
end

function [integral_approximation] = monte_carlo_integral(N, x, y,z,V)
    f = get_lake_depth(x, y);
    N_inside = sum(z > f);
    integral_approximation = V * (N_inside / length(z));
end