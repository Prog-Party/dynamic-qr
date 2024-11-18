using System.Drawing;

namespace DynamicQR.Api.Mappers;

public static class QrCodesMappers
{
    public static Application.QrCodes.Commands.CreateQrCode.Command ToCore(this Contracts.CreateQrCode.Request request)
    {
        return request is null ? null : new Application.QrCodes.Commands.CreateQrCode.Command
        {
            BackgroundColor = ColorTranslator.FromHtml(request.BackgroundColor),
            ForegroundColor = ColorTranslator.FromHtml(request.ForegroundColor),
            ImageHeight = request.ImageHeight,
            ImageUrl = request.ImageUrl,
            ImageWidth = request.ImageWidth,
            IncludeMargin = request.IncludeMargin,
            Value = request.Value
        };
    }

    public static Contracts.CreateQrCode.Response ToContract(this Application.QrCodes.Commands.CreateQrCode.Response response)
    {
        return response is null ? null : new Contracts.CreateQrCode.Response
        {
            Id = response.Id,
        };
    }

    public static Contracts.GetQrCode.Response ToContract(this Application.QrCodes.Queries.GetQrCode.Response response)
    {
        return response is null ? null : new Contracts.GetQrCode.Response
        {
            //Target = response. Target,
        };
    }

    public static Contracts.UpdateQrCodeTarget.Response ToContract(this Application.QrCodes.Commands.UpdateQrCodeTarget.Response response)
    {
        return response is null ? null : new Contracts.UpdateQrCodeTarget.Response
        {
            Id = response.Id,
        };
    }
}