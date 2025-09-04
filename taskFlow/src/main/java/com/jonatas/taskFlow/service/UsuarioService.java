package com.jonatas.taskFlow.service;

import com.jonatas.taskFlow.domain.Usuario;
import com.jonatas.taskFlow.repository.UsuarioRepository;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UsuarioService {

    private final UsuarioRepository repository;
    private final PasswordEncoder passwordEncoder;

    public UsuarioService(UsuarioRepository repository, PasswordEncoder passwordEncoder) {
        this.repository = repository;
        this.passwordEncoder = passwordEncoder;
    }

    public List<Usuario> listarTodos() {
        return repository.findAll();
    }

    public Usuario buscarPorId(Long id) {
        return repository.findById(id)
                .orElseThrow(() -> new RuntimeException("Usuário não encontrado"));
    }

    public Usuario criar(Usuario usuario) {
        if (repository.existsByEmail(usuario.getEmail())) {
            throw new RuntimeException("E-mail já cadastrado");
        }
        usuario.setSenha(passwordEncoder.encode(usuario.getSenha())); // encriptar senha
        return repository.save(usuario);
    }

    public Usuario atualizar(Long id, Usuario usuarioAtualizado) {
        Usuario usuario = buscarPorId(id);
        usuario.setUsername(usuarioAtualizado.getUsername());
        usuario.setEmail(usuarioAtualizado.getEmail());
        if (usuarioAtualizado.getSenha() != null && !usuarioAtualizado.getSenha().isEmpty()) {
            usuario.setSenha(passwordEncoder.encode(usuarioAtualizado.getSenha()));
        }
        usuario.setRole(usuarioAtualizado.getRole());
        return repository.save(usuario);
    }

    public void deletar(Long id) {
        repository.deleteById(id);
    }
}
