using FacturacionElectronica.Net.Xml.Model.Account;
using FacturacionElectronica.Net.Xml.Model.Common;
using FacturacionElectronica.Net.Xml.Model.Comprobante;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FacturacionElectronica.Net.Xml.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _loginUrl;
    private readonly string _timbrarUrl;
    private readonly string _cancelarUrl;

    public ApiService(string loginUrl, string timbrarUrl, string cancelarUrl)
    {
        _httpClient = new HttpClient();
        _loginUrl = loginUrl;
        _timbrarUrl = timbrarUrl;
        _cancelarUrl = cancelarUrl;
    }

    #region Login

    public async Task<TokenLogin> LoginAsync(Login login)
    {
        try
        {
            Console.WriteLine($"Iniciando sesión con usuario: {login.UserId}...");

            string jsonBody = JsonConvert.SerializeObject(login);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_loginUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Error HTTP {(int)response.StatusCode}: {responseBody}");

            TokenLogin? tokenLogin = JsonConvert.DeserializeObject<TokenLogin>(responseBody)
                ?? throw new ApplicationException("La respuesta del Login no pudo deserializarse.");

            if (tokenLogin.OperacionExitosa)
                Console.WriteLine("Login exitoso.");
            else
                Console.WriteLine($"Login fallido: {tokenLogin.Mensaje}");

            return tokenLogin;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error en LoginAsync: {ex.Message}", ex);
        }
    }

    #endregion

    #region Timbrar CFDI

    public async Task<TimbradoCfdiResponse> TimbrarCfdiAsync(Login login, TimbradoCfdiRequest request)
    {
        try
        {
            // Obtener Token
            TokenLogin tokenLogin = await LoginAsync(login);

            if (!tokenLogin.OperacionExitosa)
                throw new ApplicationException($"No se pudo autenticar para timbrar: {tokenLogin.Mensaje}");

            SetBearerToken(tokenLogin.Token!);

            Console.WriteLine($"Timbrando comprobante: {request.Id}...");

            string jsonBody = JsonConvert.SerializeObject(request);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_timbrarUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Error HTTP {(int)response.StatusCode}: {responseBody}");

            TimbradoCfdiResponse? timbradoResponse = JsonConvert.DeserializeObject<TimbradoCfdiResponse>(responseBody)
                ?? throw new ApplicationException("La respuesta del timbrado no pudo deserializarse.");

            if (timbradoResponse.OperacionExitosa)
                Console.WriteLine($"Timbrado exitoso. UUID: {timbradoResponse.Uuid} | PAC: {timbradoResponse.RfcPac}");
            else
                Console.WriteLine($"Error en timbrado: [{timbradoResponse.Id}] {timbradoResponse.Descripcion}");

            return timbradoResponse;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error en TimbrarCfdiAsync: {ex.Message}", ex);
        }
    }

    #endregion

    #region Cancelar CFDI

    public async Task<CancelarCfdiResponse> CancelarCfdiAsync(Login login, CancelarCfdiRequest cancelacionDTO)
    {
        try
        {
            // Obtener Token
            TokenLogin tokenLogin = await LoginAsync(login);

            if (!tokenLogin.OperacionExitosa)
                throw new ApplicationException($"No se pudo autenticar para cancelar: {tokenLogin.Mensaje}");

            SetBearerToken(tokenLogin.Token!);

            Console.WriteLine($"Cancelando comprobante UUID: {cancelacionDTO.UUID}...");

            string jsonBody = JsonConvert.SerializeObject(cancelacionDTO);
            StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_cancelarUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Error HTTP {(int)response.StatusCode}: {responseBody}");

            CancelarCfdiResponse? cancelarResponse = JsonConvert.DeserializeObject<CancelarCfdiResponse>(responseBody)
                ?? throw new ApplicationException("La respuesta de cancelación no pudo deserializarse.");

            if (cancelarResponse.OperacionExitosa)
                Console.WriteLine($"Cancelación exitosa: {cancelarResponse.Descripcion}");
            else
                Console.WriteLine($"Error en cancelación: [{cancelarResponse.Id}] {cancelarResponse.Descripcion}");

            return cancelarResponse;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error en CancelarCfdiAsync: {ex.Message}", ex);
        }
    }

    #endregion

    #region Helpers

    private void SetBearerToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    #endregion
}