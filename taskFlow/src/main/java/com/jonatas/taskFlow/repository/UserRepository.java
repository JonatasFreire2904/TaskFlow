package com.jonatas.taskFlow.repository;

import com.jonatas.taskFlow.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface UserRepository extends JpaRepository<User, Long> {
    Optional<User> findByLogin(String login);
    boolean existsByEmail(String email);
    boolean existsByLogin(String login);
}
