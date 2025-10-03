using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public ContactService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Upserts all items from ContactViewModel.ContactList.
        /// </summary>
        public async Task SaveContactAsync(ContactViewModel model)
        {
            try
            {
                if (model?.ContactList == null || model.ContactList.Count == 0)
                    return;

                foreach (var row in model.ContactList)
                {
                    if (row == null) continue;

                    Contact entity;
                    if (row.Id > 0)
                    {
                        // UPDATE path
                        entity = await _unitOfWork.Contact.GetAsync(c => c.Id == row.Id, tracked: true)
                                 ?? new Contact { Id = row.Id }; // if not found, treat as new with given Id

                        MapContact(entity, row);
                        entity.ModifiedDate = DateTime.Now;
                        entity.ModifiedBy = _env.UserId;

                        // If entity was tracked from DB, Update is fine; if it wasn't found, Update still works for attach-or-update patterns.
                        _unitOfWork.Contact.Update(entity);
                    }
                    else
                    {
                        // INSERT path
                        entity = new Contact();
                        MapContact(entity, row);

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = _env.UserId;
                        entity.ModifiedDate = DateTime.Now;
                        entity.ModifiedBy = _env.UserId;
                        entity.IsActive ??= true; // default active

                        await _unitOfWork.Contact.AddAsync(entity);
                    }
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Contacts (batch)");
                throw;
            }
        }

        // Maps SP row -> EF entity
        private static void MapContact(Contact target, GetContact_Result src)
        {
            target.Organization = src.Organization?.Trim();
            target.City = src.City?.Trim();
            target.Street_Address1 = src.Street_Address1;
            target.Street_Address2 = src.Street_Address2;
            target.Street_Address3 = src.Street_Address3;
            target.Pincode = src.Pincode;
            target.State = src.State;
            target.StateFullName = src.StateFullName;
            target.Country = src.Country;
            target.DisplayOrder = src.DisplayOrder;
            target.Phone = src.Phone;
            target.Fax = src.Fax;
            target.Email = src.Email;
            target.GoogleMapUrl = src.GoogleMapUrl;
            target.GoogleMapDirectionURL = src.GoogleMapDirectionURL;
            target.OfficeLogo = src.OfficeLogo;
            target.MapImage = src.MapImage;
            target.LeftHeading = src.LeftHeading;
            target.LeftSubHeading = src.LeftSubHeading;
            target.Longitude = src.Longitude;
            target.Lattitude = src.Lattitude;
            target.PinLabel = src.PinLabel;
            target.LinkedInUrl = src.LinkedInUrl;
            // NOTE: src (SP result) doesn’t carry IsActive/Created*/Modified* normally.
            // We set audit fields in the caller; IsActive defaulted on insert above.
        }
    }
}
