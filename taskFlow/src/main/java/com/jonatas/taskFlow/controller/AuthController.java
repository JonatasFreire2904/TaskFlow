package com.jonatas.taskFlow.controller;


import com.jonatas.taskFlow.security.JwtUtil;

import com.jonatas.taskFlow.service.UserService;
import lombok.Data;
import lombok.RequiredArgsConstructor;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/auth")
@RequiredArgsConstructor
public class AuthController {

    private final AuthenticationManager authenticationManager;
    private final UserService userService;
    private final JwtUtil jwtUtil;

    @PostMapping("/login")
    public String login(@RequestBody AuthRequest request) {
        authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(request.getLogin(), request.getSenha())
        );

        // Usar loadUserByUsername para obter UserDetails
        UserDetails userDetails = userService.loadUserByUsername(request.getLogin());
        String role = userDetails.getAuthorities().iterator().next().getAuthority();
        return jwtUtil.generateToken(userDetails.getUsername(), role);
    }

    @Data
    public static class AuthRequest {
        private String login;
        private String senha;
    }
}
