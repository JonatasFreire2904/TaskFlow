package com.jonatas.taskFlow.service;

import com.jonatas.taskFlow.entity.Project;
import com.jonatas.taskFlow.entity.Task;
import com.jonatas.taskFlow.entity.User;
import com.jonatas.taskFlow.repository.TaskRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;

@Service
@RequiredArgsConstructor
public class TaskService {

    private final TaskRepository taskRepository;

    public Task createTask(Task task) {
        if (task.getDataInicio() != null && task.getDataEntrega() != null &&
                task.getDataEntrega().isBefore(task.getDataInicio())) {
            throw new RuntimeException("Data de entrega não pode ser antes da data de início!");
        }
        return taskRepository.save(task);
    }

    public List<Task> findByProject(Project project) {
        return taskRepository.findByProjeto(project);
    }

    public List<Task> findByUser(User user) {
        return taskRepository.findByAtribuidaPara(user);
    }

    public boolean isTaskAtrasada(Task task) {
        return !task.isConcluida() &&
                task.getDataEntrega() != null &&
                task.getDataEntrega().isBefore(LocalDate.now());
    }
}