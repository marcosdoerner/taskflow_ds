import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum StatusTarefa { Pendente = 0, EmAndamento = 1, Concluida = 2, Cancelada = 3 }
export enum PrioridadeTarefa { Baixa = 0, Media = 1, Alta = 2, Urgente = 3 }

export interface Tarefa {
    id: number;
    titulo: string;
    descricao?: string;
    status: StatusTarefa;
    prioridade: PrioridadeTarefa;
    dataVencimento?: string;
    estaAtrasada: boolean;
    criadoEm: string;
    concluidoEm?: string;
}

export interface TarefaRequest {
    titulo: string;
    descricao?: string;
    prioridade: PrioridadeTarefa;
    dataVencimento?: string;
}

@Injectable({ providedIn: 'root' })
export class TarefaService {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7073/api/tarefas';

    listar(): Observable<Tarefa[]> { return this.http.get<Tarefa[]>(this.apiUrl); }
    obter(id: number): Observable<Tarefa> { return this.http.get<Tarefa>(`${this.apiUrl}/${id}`); }
    criar(req: TarefaRequest): Observable<Tarefa> { return this.http.post<Tarefa>(this.apiUrl, req); }
    atualizar(id: number, req: TarefaRequest): Observable<Tarefa> { return this.http.put<Tarefa>(`${this.apiUrl}/${id}`, req); }
    concluir(id: number): Observable<Tarefa> { return this.http.patch<Tarefa>(`${this.apiUrl}/${id}/concluir`, {}); }
    deletar(id: number): Observable<void> { return this.http.delete<void>(`${this.apiUrl}/${id}`); }

}