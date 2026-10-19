import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

export interface AuthResponse {
    token: string;
    nome: string;
    email: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7073/api/auth';
    private userSubject = new BehaviorSubject<AuthResponse | null>(null);

    constructor() {
        const stored = localStorage.getItem('user');
        if (stored) this.userSubject.next(JSON.parse(stored));
    }

    registrar(nome: string, email: string, senha: string): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/registrar`, { nome, email, senha })
            .pipe(tap(res => this.setUser(res)));
    }

    login(email: string, senha: string): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/login`, { email, senha })
            .pipe(tap(res => this.setUser(res)));

    }

    logout(): void {
        localStorage.removeItem('user');
        this.userSubject.next(null);
    }

    get token(): string | null {
        return this.userSubject.value?.token ?? null;
    }

    get isLoggedIn(): boolean {
        return !!this.token;
    }

    private setUser(res: AuthResponse): void {
        localStorage.setItem('user', JSON.stringify(res));
        this.userSubject.next(res);
    }
}