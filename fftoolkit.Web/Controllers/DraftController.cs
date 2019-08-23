using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using fftoolkit.Logic.Managers;
using fftoolkit.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace fftoolkit.Controllers
{
    public class DraftController : Controller
    {
        private DataContext _context;

        public DraftController()
        {
            _context = new DataContext();
        }

        public ActionResult Index()
        {
            DraftManager draftManager = new DraftManager(_context);
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            if (user == null)
            {
                throw new Exception("No user found;");
            }

            List<Draft> drafts = user.DraftParticipants?.Select(dp => draftManager.Get(dp.DraftId)).ToList();
            ViewData["CurrentUserId"] = user.UserId;

            return View(drafts);
        }

        public ActionResult Create()
        {
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            DateTime draftDate = DateTime.Today.AddDays(1).AddHours(17);

            Draft draft = new Draft()
            {
                OwnerUserId = user.UserId,
                StartDate = draftDate,
                Teams = 12
            };

            draft.DraftParticipants = new List<DraftParticipant>
            {
                new DraftParticipant()
                {
                    DraftId = draft.DraftId,
                    UserId = user.UserId,
                    User = user,
                    Name = HttpContext.User.Identity.Name,
                    DraftPosition = 1
                }
            };

            return View("Edit", new EditDraftViewModel()
            {
                Draft = draft,
                NewDraftInvite = new DraftInvite() { Active = true },
                NewDraftParticipant = new DraftParticipant() { DraftPosition = draft.DraftParticipants.Count + 1 },
                Leagues = user.Leagues
            });
        }

        public ActionResult Edit(int id)
        {
            DraftManager draftManager = new DraftManager(_context);
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());
            Draft draft = draftManager.Get(id) ?? throw new Exception("No draft found.");
            
            return View(new EditDraftViewModel()
            {
                Draft = draft,
                NewDraftInvite = new DraftInvite() { Active = true, DraftId = draft.DraftId },
                NewDraftParticipant = new DraftParticipant() { DraftPosition = draft.DraftParticipants.Count + 1, DraftId = draft.DraftId },
                Leagues = user.Leagues
            });
        }

        [HttpPost]
        public ActionResult Edit(EditDraftViewModel editDraftViewModel)
        {
            if (ModelState.IsValid)
            {
                DraftManager draftManager = new DraftManager(_context);
                DraftInviteManager draftInviteManager = new DraftInviteManager(_context);
                DraftParticipantManager draftParticipantManager = new DraftParticipantManager(_context);
                UserManager userManager = new UserManager(_context);
                User user = userManager.GetCurrentUser(User.Identity.GetUserId());

                if (editDraftViewModel.Draft.DraftId == 0)
                {
                    draftManager.Add(editDraftViewModel.Draft);
                }                    
                else
                {
                    List<DraftInvite> draftInvitesToAdd = editDraftViewModel.Draft.DraftInvites?.Where(di => di.DraftInviteId == 0).ToList();
                    List<DraftParticipant> draftParticipantsToAdd = editDraftViewModel.Draft.DraftParticipants?.Where(di => di.DraftParticipantId == 0).ToList();

                    if (draftInvitesToAdd != null)
                    {
                        foreach (var draftInviteToAdd in draftInvitesToAdd)
                        {
                            MailMessage mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress("fftoolkit2019@gmail.com");
                            mailMessage.To.Add(new MailAddress("soccercjs2@gmail.com"));
                            mailMessage.Body = string.Format(@"
                                You have been invited to join a fantasy football draft.
                                
                                Name: {0}
                                When: {1}

                                To join, click the following link. You will need to login or register in order to join.
                                www.fftoolkit.com/Draft/Join/{2}
                            ", editDraftViewModel.Draft.Name, editDraftViewModel.Draft.StartDate, draftInviteToAdd.Guid);

                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.Host = "smtp.gmail.com";
                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new System.Net.NetworkCredential("fftoolkit2019@gmail.com", "VonMiller58!");
                            smtpClient.Send(mailMessage);

                            draftInviteManager.Add(draftInviteToAdd);
                        }
                    }

                    if (draftParticipantsToAdd != null)
                    {
                        foreach (var draftParticipantToAdd in draftParticipantsToAdd)
                        {
                            draftParticipantManager.Add(draftParticipantToAdd);
                        }
                    }

                    draftManager.Update(editDraftViewModel.Draft);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(editDraftViewModel);
            }
        }

        public ActionResult AddLocalDrafter(EditDraftViewModel editDraftViewModel)
        {
            if (editDraftViewModel.Draft.DraftParticipants == null) { editDraftViewModel.Draft.DraftParticipants = new List<DraftParticipant>(); }
            editDraftViewModel.Draft.DraftParticipants.Add(editDraftViewModel.NewDraftParticipant);
            editDraftViewModel.NewDraftParticipant = new DraftParticipant() { DraftPosition = editDraftViewModel.Draft.DraftParticipants.Count + 1, DraftId = editDraftViewModel.Draft.DraftId };

            ModelState.Clear();

            return PartialView("_DraftInviteParticipant", editDraftViewModel);
        }

        public ActionResult AddRemoteDrafter(EditDraftViewModel editDraftViewModel)
        {
            if (editDraftViewModel.Draft.DraftInvites == null) { editDraftViewModel.Draft.DraftInvites = new List<DraftInvite>(); }

            editDraftViewModel.NewDraftInvite.Guid = Guid.NewGuid().ToString();
            editDraftViewModel.Draft.DraftInvites.Add(editDraftViewModel.NewDraftInvite);
            editDraftViewModel.NewDraftInvite = new DraftInvite() { Active = true, DraftId = editDraftViewModel.Draft.DraftId };

            ModelState.Clear();

            return PartialView("_DraftInviteParticipant", editDraftViewModel);
        }

        [Route("Draft/Join/{guid}")]
        public ActionResult Join(string guid)
        {
            DraftManager draftManager = new DraftManager(_context);
            DraftInviteManager draftInviteManager = new DraftInviteManager(_context);
            DraftParticipantManager draftParticipantManager = new DraftParticipantManager(_context);
            DraftInvite draftInvite = draftInviteManager.Get(guid);
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            if (draftInvite == null) { throw new Exception(); }
            else
            {
                Draft draft = draftManager.Get(draftInvite.DraftId);

                draftInvite.Accepted = true;
                draftInvite.Active = false;
                draftInviteManager.Update(draftInvite);

                draftParticipantManager.Add(new DraftParticipant()
                {
                    DraftId = draftInvite.DraftId,
                    UserId = user.UserId,
                    User = user,
                    Name = HttpContext.User.Identity.Name,
                    DraftPosition = draft.DraftParticipants.Count + 1
                });
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            DraftManager draftManager = new DraftManager(_context);
            draftManager.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult Room(int id)
        {
            DraftManager draftManager = new DraftManager(_context);
            PlayerManager playerManager = new PlayerManager(_context);
            TradeManager tradeManager = new TradeManager(_context);

            Draft draft = draftManager.Get(id);

            //List<Player> players = playerManager.Get(draft.League);
            //players = tradeManager.GetTradeValues(players, draft.League);
            List<Player> players = JsonConvert.DeserializeObject<List<Player>>(System.IO.File.ReadAllText(@"C:\Users\Collin\Source\Repos\fftoolkit\fftoolkit.Logic\Json Data\players.json"));
            players = players.OrderByDescending(p => p.TradeValue).ToList();
            List<string> positions = players.Select(p => p.Position).Distinct().ToList();

            return View(new DraftRoomViewModel()
            {
                Draft = draft,
                Players = players,
                Positions = positions
            });
        }
    }
}