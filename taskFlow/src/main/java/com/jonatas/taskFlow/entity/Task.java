package com.jonatas.taskFlow.entity;

import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDate;

@Entity
@Table(name = "tasks")
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Getter
@Setter

public class Task {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String titulo;
    private String descricao;

    private LocalDate dataInicio;
    private LocalDate dataEntrega;

    private boolean concluida = false;

    @ManyToOne
    @JoinColumn(name = "atribuida_para")
    private User atribuidaPara;

    @ManyToOne
    @JoinColumn(name = "projeto_id")
    private Project projeto;
}
