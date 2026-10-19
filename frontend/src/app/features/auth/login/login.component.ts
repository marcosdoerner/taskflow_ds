import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="container mt-5" style="max-width: 400px;">
      <div class="card shadow">
        <div class="card-body p-4">
          <h3 class="card-title text-center mb-4">TaskFlow</h3>
          <div *ngIf="erro" class="alert alert-danger">{{ erro }}</div>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="mb-3">
              <label class="form-label">Email</label>
              <input type="email" class="form-control" formControlName="email">
              <div *ngIf="form.get('email')?.invalid && form.get('email')?.touched"
                   class="text-danger small">Email inválido</div>
            </div>
            <div class="mb-3">
              <label class="form-label">Senha</label>
              <input type="password" class="form-control" formControlName="senha">
            </div>
            <button type="submit" class="btn btn-primary w-100"
                    [disabled]="form.invalid || carregando">
              {{ carregando ? 'Entrando...' : 'Entrar' }}
            </button>
          </form>
          <p class="text-center mt-3 mb-0">
            Não tem conta? <a routerLink="/registro">Registre-se</a>
          </p>
        </div>
      </div>
    </div>
  `
})

export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    senha: ['', Validators.required]
  });

  erro = '';
  carregando = false;

  onSubmit(): void {
    if (this.form.invalid) return;
    this.carregando = true;
    this.erro = '';

    const { email, senha } = this.form.value;
    this.auth.login(email!, senha!).subscribe({
      next: () => this.router.navigate(['/tarefas']),
      error: (err) => {
        this.erro = err.error?.mensagem ?? 'Erro ao fazer login.';
        this.carregando = false;
      }
    });
  }
}
