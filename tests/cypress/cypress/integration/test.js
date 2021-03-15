describe('todos API', () => {
    it.only('returns JSON', () => {
        cy.request({
            url: 'http://127.0.0.1:5000/api/v1/products?description=a&take=6&skip=1&isActive=true',
            headers: {
                'authorization': 'Bearer '
            }
        })
            .should((response) => {
                expect(response.status).to.eq(200)
                expect(response).to.have.property('headers')
                expect(response).to.have.property('duration')
                expect(response.headers).to.have.property('content-type')
                expect(response.body).to.have.property('total', 8)
                expect(response.body).to.have.property('pages', 2)
                expect(response.body).to.have.property('currentPage', 1)
            })
    })
})
