function [residuum_norm_direct, residuum_norm_Jacobi, residuum_norm_Gauss_Seidel] = zadanie7_solve()
    load filtr_dielektryczny.mat
    residuum_norm_direct = solve_direct(A,b);
    residuum_norm_Jacobi = solve_Jacobi(A,b);
    residuum_norm_Gauss_Seidel = solve_Gauss_Seidel(A,b);
    checkDiagonallyDominant(A)

    figure;
    
    subplot(2,1,1); 
    loglog(residuum_norm_Gauss_Seidel);
    xlabel('Iteracja');
    ylabel('Norma residuum');
    title('Metoda Gaussa-Seidla');
    grid on;

    subplot(2,1,2);
    loglog(residuum_norm_Jacobi);
    xlabel('Iteracja');
    ylabel('Norma residuum');
    title('Metoda Jacobiego');
    grid on;
    saveas(gcf, 'residuum_plot.png');

    fprintf('%e\n',residuum_norm_direct);
end