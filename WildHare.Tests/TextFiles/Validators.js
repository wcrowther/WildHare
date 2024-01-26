import { required, maxLength } from '@vuelidate/validators'

export const AccountValidator =
{
	AccountId:          {},
	AccountName:        { required, maxLength: maxLength(50) },
	Email:              { required },
	PhoneNumber:        { required },
	StreetAddress:      { required, maxLength: maxLength(50) },
	City:               { required, maxLength: maxLength(50) },
	State:              { required, maxLength: maxLength(2) },
	PostalCode:         { required, maxLength: maxLength(10) }
}

