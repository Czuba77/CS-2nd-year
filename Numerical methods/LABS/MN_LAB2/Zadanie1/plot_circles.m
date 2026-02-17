function plot_circles(a, b, circles)
    axis equal
    axis([0, a, 0, b ])
    hold on
    for i = 1:size(circles)
        plot_circle(circles(i, 3), circles(i, 1), circles(i, 2))
    end
    hold off
end