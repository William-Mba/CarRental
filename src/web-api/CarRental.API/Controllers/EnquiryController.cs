using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Storage;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryRepository _enquiryRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        public EnquiryController(IEnquiryRepository enquiryRepository, IBlobStorageService blobStorageService, IMapper mapper)
        {
            _enquiryRepository = enquiryRepository ?? throw new ArgumentNullException(nameof(enquiryRepository));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _mapper = mapper;
        }

        /// <summary>
        /// Add new customer enquiry
        /// </summary>
        /// <param name="customerEnquiry"></param>
        /// <returns>
        /// Returns created customer enquiry
        /// </returns>
        /// <response code="200">Created customer enquiry</response>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid</response>
        /// <response code="500">Opps! something went wrong</response>
        [ProducesResponseType(typeof(Enquiry), 200)]
        [HttpPost]
        public async Task<IActionResult> AddNewEnquiryAsync([FromForm] CustomerEnquiryDto customerEnquiry)
        {
            string attachementUrl = string.Empty;
            string fileTempPath = string.Empty;

            if (customerEnquiry.Attachment != null)
            {
                var fileName = $"{Guid.NewGuid()}-{customerEnquiry.Attachment.FileName}";

                fileTempPath = $@"{Path.GetTempPath()}{fileName}";

                using var stream = new FileStream(fileTempPath, FileMode.Create, FileAccess.ReadWrite);

                await customerEnquiry.Attachment.CopyToAsync(stream);

                attachementUrl = await _blobStorageService.UploadBlobAsync(stream, fileName);
            }

            var enquiry = _mapper.Map<Enquiry>(customerEnquiry);

            enquiry.AttachmentUrl = attachementUrl;

            var createdEnquiry = await _enquiryRepository.AddAsync(enquiry);

            if (!string.IsNullOrWhiteSpace(fileTempPath))
            {
                System.IO.File.Delete(fileTempPath);
            }

            return Ok(createdEnquiry);
        }
    }
}
