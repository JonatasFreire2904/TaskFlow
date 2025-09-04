package com.jonatas.taskFlow.repository;


import com.jonatas.taskFlow.entity.Project;
import com.jonatas.taskFlow.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ProjectRepository extends JpaRepository<Project, Long> {
    List<Project> findByResponsavel(User responsavel);
}
