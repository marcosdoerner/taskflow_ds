<div class="card-body" >
  <h5 class="card-title" > {{ t.titulo }}</h5>
    < p class="card-text text-muted small" * ngIf="t.descricao" > {{ t.descricao }}</p>

      < !--Status -->
        <span class="badge"
        [ngClass] = "{
'bg-secondary': t.status === 0,
  'bg-primary': t.status === 1,
    'bg-success': t.status === 2,
      'bg-dark': t.status === 3
        }">
{ { statusLabel(t.status) } }
</span>

  < !--Prioridade -->
    <span class="badge ms-1"
    [ngClass] = "{
'bg-warning text-dark': t.prioridade === 2,
  'bg-danger': t.prioridade === 3,
    'bg-secondary': t.prioridade < 2
        }">
{ { prioridadeLabel(t.prioridade) } }
</span>

  < !--Atrasada -->
    <span class="badge bg-info ms-1" * ngIf="t.estaAtrasada" > Atrasada </span>

      < !--Data de vencimento-- >
        <div class="mt-2 small" * ngIf="t.dataVencimento" >
          <span class="text-muted" > Vence em: </span>
            < strong[class.text - danger]="t.estaAtrasada" >
              {{ t.dataVencimento | date: 'dd/MM/yyyy' }}
</strong>
  </div>
  < div class="mt-2 small text-muted" * ngIf="!t.dataVencimento" >
    Sem data de vencimento
      </div>
      </div>