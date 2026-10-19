import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="container mt-5" style="max-width: 400px;">
      <div class="card shadow">
        <div class="card-body p-4">
          <h3 class="card-title text-center mb-4">Criar Conta</h3>
          <div *ngIf="erro" class="alert alert-danger">{{ erro }}</div>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="mb-3">
              <label class="form-label">Nome</label>
              <input type="text" class="form-control" formControlName="nome">
            </div>
            <div class="mb-3">
              <label class="form-label">Email</label>
              <input type="email" class="form-control" formControlName="email">
            </div>
            <div class="mb-3">
              <label class="form-label">Senha</label>
              <input type="password" class="form-control" formControlName="senha">
            </div>
            <button type="submit" class="btn btn-primary w-100"
                    [disabled]="form.invalid || carregando">
              {{ carregando ? 'Criando...' : 'Registrar' }}
            </button>
          </form>
          <p class="text-center mt-3 mb-0">
            Já tem conta? <a routerLink="/login">Entrar</a>
          </p>
        </div>
      </div>
    </div>
  `
})
export class RegistroComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', Validators.required]
  });

  erro = '';
  carregando = false;

  onSubmit(): void {
    if (this.form.invalid) return;
    this.carregando = true;
    this.erro = '';

    const { nome, email, senha } = this.form.value;
    this.auth.registrar(nome!, email!, senha!).subscribe({
      next: () => this.router.navigate(['/tarefas']),
      error: (err) => {
        this.erro = err.error?.mensagem ?? 'Erro ao registrar.';
        this.carregando = false;
      }
    });
  }
}