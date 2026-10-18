using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.UseCases.Tarefas;
using TaskFlow.Api.Extensions;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TarefasController : ControllerBase
{
    private readonly ListarTarefasUseCase _listar;
    private readonly ObterTarefaUseCase _obter;
    private readonly CriarTarefaUseCase _criar;
    private readonly AtualizarTarefaUseCase _atualizar;
    private readonly ConcluirTarefaUseCase _concluir;
    private readonly DeletarTarefaUseCase _deletar;

    public TarefasController(
        ListarTarefasUseCase listar,
        ObterTarefaUseCase obter,
        CriarTarefaUseCase criar,
        AtualizarTarefaUseCase atualizar,
        ConcluirTarefaUseCase concluir,
        DeletarTarefaUseCase deletar)
    {
        _listar = listar;
        _obter = obter;
        _criar = criar;
        _atualizar = atualizar;
        _concluir = concluir;
        _deletar = deletar;
    }

    private int UsuarioId => User.ObterUsuarioId();


    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct)
        => Ok(await _listar.ExecutarAsync(UsuarioId, ct));

    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id, CancellationToken ct)
    {
        var tarefa = await _obter.ExecutarAsync(id, UsuarioId, ct);
        return tarefa == null ? NotFound() : Ok(tarefa);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] TarefaRequest request, CancellationToken ct)
    {
        var tarefa = await _criar.ExecutarAsync(request, UsuarioId, ct);
        return CreatedAtAction(nameof(Obter), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] TarefaRequest request, CancellationToken ct)
    {
        var tarefa = await _atualizar.ExecutarAsync(id, request, UsuarioId, ct);
        return tarefa == null ? NotFound() : Ok(tarefa);
    }

    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> Concluir(int id, CancellationToken ct)
    {
        var tarefa = await _concluir.ExecutarAsync(id, UsuarioId, ct);
        return tarefa == null ? NotFound() : Ok(tarefa);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id, CancellationToken ct)
        => await _deletar.ExecutarAsync(id, UsuarioId, ct) ? NoContent() : NotFound();

}
