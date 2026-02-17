package entity;

import jakarta.persistence.*;
import lombok.*;

@NoArgsConstructor
@Entity
public class Kotek {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Getter @Setter
    private Long id;
    @Getter @Setter
    private String name;
    @Getter @Setter
    private int age;
    @Getter @Setter
    private double weight;

    @ManyToOne @Getter @Setter
    private Dom dom;

    public Kotek(String name, int age, double weight, Dom dom) {
        this.name = name;
        this.age = age;
        this.weight = weight;
        this.dom = dom;
    }


}
