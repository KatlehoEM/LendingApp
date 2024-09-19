import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home/home.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { ToastrModule } from 'ngx-toastr';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BorrowerDashboardComponent } from './components/borrower-dashboard/borrower-dashboard.component';
import { LenderDashboardComponent } from './components/lender-dashboard/lender-dashboard.component';
import { LoanOffersComponent } from './components/loan-offers/loan-offers.component';
import { JwtInterceptor } from './_interceptor/jwt.interceptor';
import { CreateOfferComponent } from './components/create-offer/create-offer.component';
import { LoanapplicationComponent } from './components/loanapplication/loanapplication.component';
import { PaymentModalComponentComponent } from './components/payment-modal-component/payment-modal-component.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { StarRatingComponent } from './components/star-rating/star-rating.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SidebarComponent,
    RegisterComponent,
    LoginComponent,
    BorrowerDashboardComponent,
    LenderDashboardComponent,
    LoanOffersComponent,
    CreateOfferComponent,
    LoanapplicationComponent,
    PaymentModalComponentComponent,
    StarRatingComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-center'
    }),
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS,useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
