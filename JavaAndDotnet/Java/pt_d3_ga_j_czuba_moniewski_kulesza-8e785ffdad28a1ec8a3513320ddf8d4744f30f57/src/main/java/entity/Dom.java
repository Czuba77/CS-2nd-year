package entity;

import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@NoArgsConstructor
@Entity
public class Dom {
    @Getter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Getter
    @Setter
    private String name;

    @Getter
    @Setter
    @OneToMany(mappedBy = "dom", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Kotek> Kotki;

    public Dom(String name_) {
        this.name = name_;
    }
}
