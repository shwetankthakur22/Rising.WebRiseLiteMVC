 $(document).ready(function () {
           $('.bankdetails').click(function () {
               $("#code").val("").html("");
               $("#name").val("").html("");
               $("#micr").val("").html("");
               $("#rrid").val("").html("");

               var id = $(this).data("model-id");
               debugger
               $.ajax({
                   url: '/Master/GetBankDetails/',
                   type: "GET",
                   dataType: "JSON",
                   data: { rid: id },
                   success: function (bank) {
                       debugger
                       $("#bcode").val(bank.BankCode).html(bank.BankCode);
                       $("#bname").val(bank.BankName).html(bank.BankName);
                       $("#micr").val(bank.Micr).html(bank.Micr);
                       $("#rwid").val(bank.Rwid).html(bank.Rwid);
                       $('#BankCodeFrom').modal('hide');

                   }
              });

       });
 });


 $(document).ready(function () {
     $('.clientmaster').click(function () {
         var id = $(this).data("model-id");
         debugger
         $.ajax({
             url: '/Master/GetClientDetails/',
             type: "GET",
             dataType: "JSON",
             data: { rid: id },
             success: function (csn) {
                 debugger
                 const d = new Date(csn.ExpiryDate.match(/\d+/)[0] * 1);
                 const formattedDate = d.getFullYear() + '-' + ("0" + (d.getMonth() + 1)).slice(-2) + '-' + ("0" + d.getDate()).slice(-2)
                 $('#date-end').val(formattedDate);              
                 $("#clientcode").val(csn.ClientCode).html(csn.ClientCode);
                 $("#name").val(csn.Name).html(csn.Name);
                 $("#fname").val(csn.FatherName).html(csn.FatherName);
                 $("#add1").val(csn.Address1).html(csn.Address1);
                 $("#add2").val(csn.Address2).html(csn.Address2);
                 $("#add3").val(csn.Address3).html(csn.Address3);
                 $("#padd1").val(csn.PAddress1).html(csn.PAddress1);
                 $("#padd2").val(csn.PAddress2).html(csn.PAddress2);
                 $("#padd3").val(csn.PAddress3).html(csn.PAddress3);
                 $("#pincode").val(csn.PinCode).html(csn.PinCode);
                 $("#dob").val(csn.Dob).html(csn.Dob);
                 $("#phone").val(csn.Phone).html(csn.Phone);
                 $("#emailid").val(csn.EmailId).html(csn.EmailId);
                 $("#city").val(csn.City).html(csn.City);
                 $("#state").val(csn.State).html(csn.State);
                 $("#statecode").val(csn.StateCode).html(csn.StateCode);
                 $("#country").val(csn.Country).html(csn.Country);
                 $("#group").val(csn.Group).html(csn.Group);
                 $("#gender").val(csn.Gender).change();
                 $("#martial").val(csn.Martial).change();
                 $("#accgrp").val(csn.AccountGroup).html(csn.AccountGroup);
                 $("#branchcode").val(csn.BranchCode).html(csn.BranchCode);
                 $("#subbranch").val(csn.SubBranch).html(csn.SubBranch);
                 $("#panno").val(csn.PanNo).html(csn.PanNo);
                 $("#contract").val(csn.ContractType).html(csn.ContractType);
                 $("#clientenable").val(csn.ClientEnable).html(csn.ClientEnable);
                 $("#rmcode").val(csn.RMCode).html(csn.RMCode);
                 $("#cin").val(csn.CIN).html(csn.CIN);
                 $("#reason").val(csn.Reason).html(csn.Reason);
                 $("#remark").val(csn.Remark).html(csn.Remark);

                 $("#exch").val(csn.Exchange).change();
                 $("#shortcode").val(csn.ShortCode).html(csn.ShortCode);
                 $("#brkrage").val(csn.BrokerageMethod).html(csn.BrokerageMethod);
                 $("#contractg").val(csn.ContractG).html(csn.ContractG);
                 $("#tax1").val(csn.Tax1).html(csn.Tax1);
                 $("#tax2").val(csn.Tax2).html(csn.Tax2);
                 $("#tax3").val(csn.Tax3).html(csn.Tax3);
                 $("#tax4").val(csn.Tax4).html(csn.Tax4);
                 $("#cashacc").val(csn.CashAcc).html(csn.CashAcc);
                 $("#dailymtmac").val(csn.DailyMTMAc).html(csn.DailyMTMAc);
                 $("#dailymargin").val(csn.DailyMarginAcc).html(csn.DailyMarginAcc);
                 $("#interestapp").val(csn.InterestApp).html(csn.InterestApp);
                 $("#interest").val(csn.Interest).html(csn.Interest);
                 $("#interestamt").val(csn.Interestamt).html(csn.Interestamt);
                 $("#securityacc").val(csn.Securityacc).html(csn.Securityacc);
                 $("#openingbal").val(csn.OpeningBal).html(csn.OpeningBal);
                 $("#dealercd").val(csn.Dealercd).html(csn.Dealercd);
                 $("#brokrage").val(csn.brokerageutd).html(csn.brokerageutd);
                 $("#transactionin").val(csn.Transactioin).html(csn.Transactioin);
                 $("#stampda").val(csn.stampda).html(csn.stampda);
                 $("#marginpost").val(csn.marginposting).html(csn.marginposting);
                 $("#custodian").val(csn.Custodian).html(csn.Custodian);
                 $("#introducercd").val(csn.introducercd).html(csn.introducercd);
                 $("#uccupload").val(csn.uccupload).html(csn.uccupload);
                 $("#brokapp").val(csn.brokapp).html(csn.brokapp);

                 $("#bankacc").val(csn.bankacc).html(csn.bankacc);
                 $("#actype").val(csn.actype).html(csn.actype);
                 $("#bankname").val(csn.bankname).html(csn.bankname);
                 $("#baddress1").val(csn.baddress1).html(csn.baddress1);
                 $("#micr").val(csn.micr).html(csn.micr);
                 $("#ifsc").val(csn.ifsc).html(csn.ifsc);
                 $("#incdate").val(csn.incdate).html(csn.incdate);
                 $("#regno").val(csn.regno).html(csn.regno);
                 $("#regauth").val(csn.regauth).html(csn.regauth);
                 $("#regplace").val(csn.regplace).html(csn.regplace);
                 $("#contactp1").val(csn.contactp1).html(csn.contactp1);
                 $("#contactp2").val(csn.contactp2).html(csn.contactp2);
                 $("#contactp1deg").val(csn.contactp1deg).html(csn.contactp1deg);
                 $("#contactp2deg").val(csn.contactp2deg).html(csn.contactp2deg);
                 $("#contactp1add").val(csn.contactp1add).html(csn.contactp1add);
                 $("#contactp2add").val(csn.contactp2add).html(csn.contactp2add);
                 $("#contactp2phn").val(csn.contactp2phn).html(csn.contactp2phn);
                 $("#contactp2email").val(csn.contactp2email).html(csn.contactp2email);
                 $("#contactp2pan").val(csn.contactp2pan).html(csn.contactp2pan);
                 $("#directnm").val(csn.directnm).html(csn.directnm);
                 $("#directadd").val(csn.directadd).html(csn.directadd);
                 $("#directphn").val(csn.directphn).html(csn.directphn);
                 $("#dirpanno").val(csn.dirpanno).html(csn.dirpanno);
                 $("#diremail").val(csn.DirEmail).html(csn.DirEmail);
                 $("#din1").val(csn.Din1).html(csn.Din1);
                 $("#din2").val(csn.Din2).html(csn.Din2);
                 $("#rwid").val(csn.Rwid).html(csn.Rwid);
                 $('#ClientCode').modal('hide');
             }
         });

     });
 });

