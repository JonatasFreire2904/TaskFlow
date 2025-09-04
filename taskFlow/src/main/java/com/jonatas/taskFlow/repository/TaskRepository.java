package com.jonatas.taskFlow.repository;

import com.jonatas.taskFlow.entity.Project;
import com.jonatas.taskFlow.entity.Task;
import com.jonatas.taskFlow.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface TaskRepository extends JpaRepository<Task, Long> {
    List<Task> findByProjeto(Project projeto);
    List<Task> findByAtribuidaPara(User user);
}
