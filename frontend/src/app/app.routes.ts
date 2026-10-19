import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login.component')
            .then(m => m.LoginComponent)
    },
    {
        path: 'registro',
        loadComponent: () => import('./features/auth/registro/registro.component')
            .then(m => m.RegistroComponent)
    },
    {
        path: 'tarefas',
        loadComponent: () => import('./features/tarefas/lista-tarefas/lista-tarefas.component')
            .then(m => m.ListaTarefasComponent),
        canActivate: [authGuard]
    },
    { path: '', redirectTo: '/tarefas', pathMatch: 'full' },
    { path: '**', redirectTo: '/tarefas' }
];
