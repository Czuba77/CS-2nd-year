function [matrix_sizes, condition_numbers, interpolation_error_exact, interpolation_error_perturbed] = ...
        vpa_ill_conditioning_demo()

% Określa wpływ współczynnika uwarunkowania macierzy Vandermonde'a na dokładność interpolacji.
% Generuje trzy wykresy ilustrujące uwarunkowanie macierzy i błędy interpolacji.
% W obliczeniach stosuje arytmetykę zmiennoprzecinkową o wyższej precyzji.
%
% matrix_sizes - rozmiar testowych macierzy Vandermonde'a
% condition_numbers - współczynniki uwarunkowania testowych macierzy Vandermonde'a
% interpolation_error_exact - maksymalna różnica między referencyjnymi
%       a obliczonymi współczynnikami wielomianu, gdy b zawiera
%       wartości funkcji kwadratowej 
% interpolation_error_perturbed - maksymalna różnica między referencyjnymi
%       a obliczonymi współczynnikami wielomianu, gdy b zawiera
%       zaburzone wartości funkcji kwadratowej 

    a = vpa(-1);
    b = vpa(1);

    digits(50);

    matrix_sizes = 4:4:100;
    num_points = length(matrix_sizes);

    condition_numbers = zeros(1, num_points);
    interpolation_error_exact = zeros(1, num_points);
    interpolation_error_perturbed = zeros(1, num_points);

    %===========================================================================
    % Część 1: Obliczanie współczynnika uwarunkowania macierzy Vandermonde'a
    %===========================================================================
    for index = 1:num_points
        size_n = vpa(matrix_sizes(index));
        indices = vpa(0:size_n-1)';
        interpolation_nodes = a + indices * (b - a) / vpa(size_n - 1);

        V = get_vandermonde_matrix(interpolation_nodes);

        condition_numbers(index) = double(cond(V)); % TODO
    end

    threshold_index = find(condition_numbers >= 1e8, 1);

    tiledlayout(3, 1, 'Padding', 'compact', 'TileSpacing', 'compact');
    nexttile;
    semilogy(matrix_sizes, condition_numbers, 'LineWidth', 2); % semilogy(matrix_sizes, % TODO
    hold on;
    if ~isempty(threshold_index)
        size_threshold = matrix_sizes(threshold_index);
        xline(size_threshold, ':', 'cond(V) > 10^8', 'LabelOrientation', ...
            'horizontal', 'LabelVerticalAlignment', 'top', ...
            'LabelHorizontalAlignment', 'left', 'LineWidth', 2, ...
            'Color', [0.494 0.184 0.556]);
    end
    hold off;
    xlabel('Rozmiar macierzy');
    ylabel('Współczynnik uwarunkowania');
    title('Uwarunkowanie macierzy Vandermonde');

    %===========================================================================
    % Część 2: Obliczenie błędu interpolacji dla dokładnych danych (f(x)=x^2)
    %===========================================================================
    for index = 1:num_points
        size_n = matrix_sizes(index);

        indices = vpa(0:size_n-1)';
        interpolation_nodes = a + indices * (b - a) / vpa(size_n - 1);

        V = get_vandermonde_matrix(interpolation_nodes);
        
        a2 = vpa(1);
        b_exact = a2 * interpolation_nodes.^vpa(2);
        reference_coefficients = [vpa(0); vpa(0); a2; vpa(zeros(size_n-3, 1))];
        computed_coefficients = V \ b_exact;

        interpolation_error_exact(index) = double(max(abs(computed_coefficients - reference_coefficients)));
    end

    nexttile;
    semilogy(matrix_sizes, interpolation_error_exact, 'LineWidth', 2); % plot(matrix_sizes, % TODO
    xlabel('Rozmiar macierzy');
    ylabel('Błąd interpolacji');
    title('Błąd interpolacji dla danych dokładnych');

    %===========================================================================
    % Część 3: Obliczenie błędu interpolacji dla danych zaburzonych
    %===========================================================================
    for index = 1:num_points
        size_n = matrix_sizes(index);
        size_n_vpa = vpa(size_n);
        interpolation_nodes = a + (0:size_n_vpa-1)' * (b - a) / (size_n_vpa - 1);
        V = get_vandermonde_matrix(interpolation_nodes);

        a2 = vpa(1);
        noise = vpa(rand(size_n, 1) * 1e-9);
        b_perturbed = a2 * interpolation_nodes.^vpa(2) + noise;
        reference_coefficients = [0; 0; a2; zeros(size_n - 3, 1)];
        computed_coefficients = V \ b_perturbed;

        interpolation_error_perturbed(index) = double(max(abs(computed_coefficients - reference_coefficients)));
    end

    nexttile;
    semilogy(matrix_sizes, interpolation_error_perturbed, 'LineWidth', 2); % TODO:
    xlabel('Rozmiar macierzy');
    ylabel('Błąd interpolacji');
    title('Błąd interpolacji dla danych zaburzonych');
    yticks([1e-10 1e-5 1 1e5 1e10 1e15 1e20 1e25 1e30 1e35]); % yticks([1 1e20 1e35])
    saveas(gcf,'zadanie4.png');
end

%function V = get_vandermonde_matrix(x)
    % Buduje macierz Vandermonde’a na podstawie wektora węzłów interpolacji x.
%    N = length(x);
%    V = vpa(zeros(N,N));  % Inicjalizacja jako macierz zmiennych vpa
%    for i = 1:N
%        for j = 1:N
%            V(i, j) = x(i)^(j-1);
%        end
%    end
%end

function V = get_vandermonde_matrix(x)
    % Buduje macierz Vandermonde’a na podstawie wektora węzłów interpolacji x.
    N = length(x);
    for i=1 :N
       V(i,:)=x.^(i-1);
    end
end


%function V = get_vandermonde_matrix(x)
%    % Buduje macierz Vandermonde’a na podstawie wektora węzłów interpolacji x.
%    N = length(x);
%   x = x(:);
%    powers = vpa(0:N-1);
%    V = x .^ powers; 
%end