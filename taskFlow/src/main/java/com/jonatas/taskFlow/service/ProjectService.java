package com.jonatas.taskFlow.service;

import com.jonatas.taskFlow.entity.Project;
import com.jonatas.taskFlow.entity.User;
import com.jonatas.taskFlow.repository.ProjectRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class ProjectService {

    private final ProjectRepository projectRepository;

    public Project createProject(Project project) {
        return projectRepository.save(project);
    }

    public List<Project> getProjectsByResponsavel(User responsavel) {
        return projectRepository.findByResponsavel(responsavel);
    }

    public List<Project> findAll() {
        return projectRepository.findAll();
    }
}
