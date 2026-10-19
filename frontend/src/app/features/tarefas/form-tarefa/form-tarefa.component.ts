import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Tarefa, TarefaRequest, PrioridadeTarefa } from '../../../core/services/tarefa.service';

@Component({
  selector: 'app-form-tarefa',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="modal fade show d-block" tabindex="-1" style="background: rgba(0,0,0,.5);">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ tarefa ? 'Editar Tarefa' : 'Nova Tarefa' }}</h5>
            <button type="button" class="btn-close" (click)="cancelar.emit()"></button>
          </div>

          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Título *</label>
                <input type="text" class="form-control" formControlName="titulo"
                       [class.is-invalid]="form.get('titulo')?.invalid && form.get('titulo')?.touched">
                <div class="invalid-feedback">Título é obrigatório (máx. 200 caracteres).</div>
              </div>

              <div class="mb-3">
                <label class="form-label">Descrição</label>
                <textarea class="form-control" rows="3" formControlName="descricao"></textarea>
              </div>

              <div class="row">
                <div class="col-md-6 mb-3">
                  <label class="form-label">Prioridade</label>
                  <select class="form-select" formControlName="prioridade">
                    <option [ngValue]="0">Baixa</option>
                    <option [ngValue]="1">Média</option>
                    <option [ngValue]="2">Alta</option>
                    <option [ngValue]="3">Urgente</option>
                  </select>
                </div>

                <div class="col-md-6 mb-3">
                  <label class="form-label">Vencimento</label>
                  <input type="date" class="form-control" formControlName="dataVencimento">
                </div>
              </div>

              <div *ngIf="erro" class="alert alert-danger py-2 small">{{ erro }}</div>
            </div>

            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" (click)="cancelar.emit()">
                Cancelar
              </button>
              <button type="submit" class="btn btn-primary"
                      [disabled]="form.invalid || salvando">
                {{ salvando ? 'Salvando...' : 'Salvar' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `
})
export class FormTarefaComponent implements OnChanges {
  @Input() tarefa: Tarefa | null = null;
  @Input() salvando = false;
  @Input() erro = '';
  @Output() salvar = new EventEmitter<TarefaRequest>();
  @Output() cancelar = new EventEmitter<void>();

  private fb = inject(FormBuilder);

  form = this.fb.group({
    titulo: ['', [Validators.required, Validators.maxLength(200)]],
    descricao: [''],
    prioridade: [PrioridadeTarefa.Media as number, Validators.required],
    dataVencimento: ['']
  });

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tarefa']) {
      this.preencherForm();
    }
  }

  private preencherForm(): void {
    if (this.tarefa) {
      this.form.patchValue({
        titulo: this.tarefa.titulo,
        descricao: this.tarefa.descricao ?? '',
        prioridade: this.tarefa.prioridade,
        dataVencimento: this.tarefa.dataVencimento
          ? this.tarefa.dataVencimento.substring(0, 10)
          : ''
      });
    } else {
      this.form.reset({ prioridade: PrioridadeTarefa.Media });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const request: TarefaRequest = {
      titulo: raw.titulo!.trim(),
      descricao: raw.descricao?.trim() || undefined,
      prioridade: raw.prioridade as PrioridadeTarefa,
      dataVencimento: raw.dataVencimento
        ? new Date(raw.dataVencimento).toISOString()
        : undefined
    };

    this.salvar.emit(request);
  }
}